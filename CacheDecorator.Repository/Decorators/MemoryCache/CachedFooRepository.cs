using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CacheDecorator.Common;
using CacheDecorator.Common.Caching;
using CacheDecorator.Common.Settings;
using CacheDecorator.Repository.Entities;
using CacheDecorator.Repository.Interface;
using CoreProfiler;

namespace CacheDecorator.Repository.Decorators.MemoryCache
{
    /// <summary>
    /// class CachedFooRepository
    /// </summary>
    public class CachedFooRepository : MemoryCacheRepositoryBase, IFooRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CachedFooRepository"/> class.
        /// </summary>
        /// <param name="cacheDecoratorSettingsProvider">The cacheDecoratorSettingsProvider.</param>
        /// <param name="cacheProviderResolver">The cacheProviderResolver</param>
        /// <param name="fooRepository">The fooRepository</param>
        public CachedFooRepository(ICacheDecoratorSettingsProvider cacheDecoratorSettingsProvider,
                                   ICacheProviderResolver cacheProviderResolver,
                                   IFooRepository fooRepository)
            : base(cacheDecoratorSettingsProvider, cacheProviderResolver)
        {
            this.SetCacheProvider
            (
                cacheType: CacheTypeEnum.Memory,
                declaration: nameof(IFooRepository),
                implement: nameof(CachedFooRepository)
            );

            this.FooRepository = fooRepository;
        }

        private IFooRepository FooRepository { get; set; }

        //-----------------------------------------------------------------------------------------

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public async Task<IResult> InsertAsync(FooModel model)
        {
            var stepName = $"{nameof(CachedFooRepository)}.{nameof(this.InsertAsync)}";
            using (ProfilingSession.Current.Step(stepName))
            {
                var result = await this.FooRepository.InsertAsync(model);

                this.RemoveCacheItem(Cachekeys.Foo.Exists.ToFormat(model.FooId));
                this.RemoveCacheItem(Cachekeys.Foo.Get.ToFormat(model.FooId));

                return result;
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public async Task<IResult> UpdateAsync(FooModel model)
        {
            var stepName = $"{nameof(CachedFooRepository)}.{nameof(this.UpdateAsync)}";
            using (ProfilingSession.Current.Step(stepName))
            {
                var result = await this.FooRepository.UpdateAsync(model);

                this.RemoveCacheItem(Cachekeys.Foo.Exists.ToFormat(model.FooId));
                this.RemoveCacheItem(Cachekeys.Foo.Get.ToFormat(model.FooId));

                return result;
            }
        }

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var stepName = $"{nameof(CachedFooRepository)}.{nameof(this.DeleteAsync)}";
            using (ProfilingSession.Current.Step(stepName))
            {
                var result = await this.FooRepository.DeleteAsync(id);

                if (result.Success.Equals(false))
                {
                    return result;
                }

                this.RemoveCacheItem(Cachekeys.Foo.Exists.ToFormat(id));
                this.RemoveCacheItem(Cachekeys.Foo.Get.ToFormat(id));

                return result;
            }
        }

        /// <summary>
        /// 以 FooId 確認資料是否存在
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public async Task<bool> IsExistsAsync(Guid id)
        {
            var stepName = $"{nameof(CachedFooRepository)}.{nameof(this.IsExistsAsync)}";
            using (ProfilingSession.Current.Step(stepName))
            {
                var cacheItem = await this.GetOrAddCacheItemAsync
                (
                    Cachekeys.Foo.Exists.ToFormat(id),
                    CacheUtility.GetCacheItemExpiration5Min(),
                    async () =>
                    {
                        var result = await this.FooRepository.IsExistsAsync(id);
                        return result;
                    }
                );

                return cacheItem;
            }
        }

        /// <summary>
        /// 以 Id 取得資料
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Foo.</returns>
        public async Task<FooModel> GetAsync(Guid id)
        {
            var stepName = $"{nameof(CachedFooRepository)}.{nameof(this.GetAsync)}";
            using (ProfilingSession.Current.Step(stepName))
            {
                var cacheItem = await this.GetOrAddCacheItemAsync
                (
                    cachekey: Cachekeys.Foo.Get.ToFormat(id),
                    cacheItemExpiration: CacheUtility.GetCacheItemExpiration5Min(),
                    source: async () =>
                    {
                        var result = await this.FooRepository.GetAsync(id);
                        return result;
                    }
                );

                return cacheItem;
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
            var stepName = $"{nameof(CachedFooRepository)}.{nameof(this.GetTotalCountAsync)}";
            using (ProfilingSession.Current.Step(stepName))
            {
                var result = await this.FooRepository.GetTotalCountAsync();
                return result;
            }
        }

        /// <summary>
        /// 取得指定範圍與數量的資料
        /// </summary>
        /// <param name="from">The from.</param>
        /// <param name="size">The size.</param>
        /// <param name="displayAll">if set to <c>true</c> [display all].</param>
        /// <returns>
        /// List&lt;Foo&gt;.
        /// </returns>
        public async Task<List<FooModel>> GetCollectionAsync(int @from, int size, bool displayAll = false)
        {
            var stepName = $"{nameof(CachedFooRepository)}.{nameof(this.GetCollectionAsync)}";
            using (ProfilingSession.Current.Step(stepName))
            {
                var result = await this.FooRepository.GetCollectionAsync(@from, size, displayAll);
                return result;
            }
        }
    }
}