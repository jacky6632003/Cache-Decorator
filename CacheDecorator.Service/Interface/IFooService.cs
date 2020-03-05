using CacheDecorator.Common;
using CacheDecorator.Service.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CacheDecorator.Service.Interface
{
    /// <summary>
    /// interface IFooService.
    /// </summary>
    public interface IFooService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        Task<IResult> InsertAsync(FooObject model);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        Task<IResult> UpdateAsync(FooObject model);

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        Task<IResult> DeleteAsync(Guid id);

        /// <summary>
        /// 取得資料總數
        /// </summary>
        /// <param name="displayAll">if set to <c>true</c> [display all].</param>
        /// <returns>System.Int32.</returns>
        Task<int> GetTotalCountAsync(bool displayAll = false);

        /// <summary>
        /// 取得全部 FooObject
        /// </summary>
        /// <param name="displayAll">if set to <c>true</c> [display all].</param>
        /// <returns></returns>
        Task<List<FooObject>> GetAllAsync(bool displayAll = false);

        /// <summary>
        /// 以 Id 檢查資料是否存在
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        Task<bool> IsExistsAsync(Guid id);

        /// <summary>
        /// 以 Id 取得資料
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>FooObject.</returns>
        Task<FooObject> GetAsync(Guid id);

        /// <summary>
        /// 取得指定範圍與數量的資料
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="size">The size.</param>
        /// <param name="displayAll">if set to <c>true</c> [display all].</param>
        /// <returns>
        /// List&lt;SystemAdminDto&gt;.
        /// </returns>
        Task<List<FooObject>> GetCollectionAsync(int @from, int size, bool displayAll = false);
    }
}