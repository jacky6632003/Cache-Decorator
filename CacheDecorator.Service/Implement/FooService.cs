using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CacheDecorator.Common;
using CacheDecorator.Repository.Entities;
using CacheDecorator.Repository.Interface;
using CacheDecorator.Service.Interface;
using CacheDecorator.Service.Model;
using CoreProfiler;
using Microsoft.Extensions.Logging;

namespace CacheDecorator.Service.Implement
{
    /// <summary>
    /// class FooService
    /// </summary>
    /// <seealso cref="IFooService" />
    public class FooService : IFooService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FooService" /> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logHelperFactory">The log helper factory.</param>
        /// <param name="fooRepository">The Foo repository.</param>
        public FooService(IMapper mapper,

                          IFooRepository fooRepository)
        {
            this.Mapper = mapper;

            this.FooRepository = fooRepository;
        }

        private IMapper Mapper { get; set; }

        private IFooRepository FooRepository { get; set; }

        //-----------------------------------------------------------------------------------------

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        ///   <c>true</c> if XXXX, <c>false</c> otherwise.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">model</exception>
        public async Task<IResult> InsertAsync(FooObject model)
        {
            var stepName = $"{nameof(FooService)}.InsertAsync";
            using (ProfilingSession.Current.Step(stepName))
            {
                var startTime = SystemTime.UtcNow;

                if (model.EqualNull())
                {
                    throw new ArgumentNullException(nameof(model));
                }

                // 檢查名稱是否與現有資料衝突
                var totalCount = await this.FooRepository.GetTotalCountAsync(true);
                if (totalCount > 0)
                {
                    var collection = await this.FooRepository.GetCollectionAsync(1, totalCount, true);
                    if (collection.Any(x => x.Name.Equals(model.Name, StringComparison.OrdinalIgnoreCase)))
                    {
                        return new Result(false)
                        {
                            Message = "已有重複名稱的資料存在"
                        };
                    }
                }

                if (model.FooId.Equals(Guid.Empty))
                {
                    model.FooId = Guid.NewGuid();
                }

                // 新增資料
                var fooModel = this.Mapper.Map<FooObject, FooModel>(model);
                var result = await this.FooRepository.InsertAsync(fooModel);

                return result;
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        ///   <c>true</c> if XXXX, <c>false</c> otherwise.
        /// </returns>
        /// <exception cref="System.ArgumentException">model</exception>
        public async Task<IResult> UpdateAsync(FooObject model)
        {
            var stepName = $"{nameof(FooService)}.UpdateAsync";
            using (ProfilingSession.Current.Step(stepName))
            {
                var startTime = SystemTime.UtcNow;

                if (model.EqualNull())
                {
                    throw new ArgumentNullException(nameof(model));
                }

                IResult result = new Result(false);

                var exists = await this.IsExistsAsync(model.FooId);
                if (exists.Equals(false))
                {
                    result.Message = "無法找到 id 對應的資料";
                    return result;
                }

                var fooModel = this.Mapper.Map<FooObject, FooModel>(model);

                result = await this.FooRepository.UpdateAsync(fooModel);

                return result;
            }
        }

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        ///   <c>true</c> if XXXX, <c>false</c> otherwise.
        /// </returns>
        /// <exception cref="System.ArgumentException">id</exception>
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var stepName = $"{nameof(FooService)}.DeleteAsync";
            using (ProfilingSession.Current.Step(stepName))
            {
                var startTime = SystemTime.UtcNow;

                if (id.Equals(Guid.Empty))
                {
                    throw new ArgumentException(nameof(id));
                }

                var exists = await this.IsExistsAsync(id);
                if (exists.Equals(false))
                {
                    return new Result(false)
                    {
                        Message = "資料不存在"
                    };
                }

                var foo = await this.GetAsync(id);

                var result = await this.FooRepository.DeleteAsync(id);

                return result;
            }
        }

        /// <summary>
        /// 取得資料總數
        /// </summary>
        /// <param name="displayAll">if set to <c>true</c> [display all].</param>
        /// <returns>
        /// System.Int32.
        /// </returns>
        public async Task<int> GetTotalCountAsync(bool displayAll = false)
        {
            var stepName = $"{nameof(FooService)}.GetTotalCountAsync";
            using (ProfilingSession.Current.Step(stepName))
            {
                var count = await this.FooRepository.GetTotalCountAsync(displayAll);
                return count;
            }
        }

        /// <summary>
        /// 取得全部 Foo 資料
        /// </summary>
        /// <param name="displayAll">if set to <c>true</c> [display all].</param>
        /// <returns></returns>
        public async Task<List<FooObject>> GetAllAsync(bool displayAll = false)
        {
            var stepName = $"{nameof(FooService)}.GetAllAsync";
            using (ProfilingSession.Current.Step(stepName))
            {
                var total = await this.FooRepository.GetTotalCountAsync(displayAll);

                if (total.Equals(0))
                {
                    return new List<FooObject>();
                }

                var models = await this.FooRepository.GetCollectionAsync
                (
                    1, total, displayAll
                );

                var foos = this.Mapper.Map<List<FooModel>, List<FooObject>>(models);
                return foos;
            }
        }

        /// <summary>
        /// 以 Id 檢查資料是否存在
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">id</exception>
        public async Task<bool> IsExistsAsync(Guid id)
        {
            var stepName = $"{nameof(FooService)}.IsExistsAsync";
            using (ProfilingSession.Current.Step(stepName))
            {
                if (id.Equals(Guid.Empty))
                {
                    throw new ArgumentException(nameof(id));
                }

                var result = await this.FooRepository.IsExistsAsync(id);
                return result;
            }
        }

        /// <summary>
        /// 以 Id 取得資料
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// Foo.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">id</exception>
        public async Task<FooObject> GetAsync(Guid id)
        {
            var stepName = $"{nameof(FooService)}.GetAsync";
            using (ProfilingSession.Current.Step(stepName))
            {
                if (id.Equals(Guid.Empty))
                {
                    throw new ArgumentException(nameof(id));
                }

                var model = await this.FooRepository.GetAsync(id);
                if (model.EqualNull())
                {
                    return null;
                }

                var foo = this.Mapper.Map<FooModel, FooObject>(model);
                return foo;
            }
        }

        /// <summary>
        /// 取得指定範圍與數量的資料
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="size">The size.</param>
        /// <param name="displayAll">if set to <c>true</c> [display all].</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// from
        /// or
        /// size
        /// </exception>
        public async Task<List<FooObject>> GetCollectionAsync(int @from, int size, bool displayAll = false)
        {
            var stepName = $"{nameof(FooService)}.GetCollectionASync";
            using (ProfilingSession.Current.Step(stepName))
            {
                if (@from <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(@from));
                }

                if (size <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(size));
                }

                var totalCount = await this.GetTotalCountAsync(displayAll);
                if (totalCount.Equals(0))
                {
                    return new List<FooObject>();
                }

                if (from > totalCount)
                {
                    return new List<FooObject>();
                }

                var models = await this.FooRepository.GetCollectionAsync(from, size, displayAll);

                if (models.Any().Equals(false))
                {
                    return new List<FooObject>();
                }

                var foos = this.Mapper.Map<List<FooModel>, List<FooObject>>(models);
                return foos;
            }
        }
    }
}