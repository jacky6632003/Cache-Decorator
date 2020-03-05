using CacheDecorator.Common;
using CacheDecorator.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CacheDecorator.Repository.Interface
{
    /// <summary>
    /// Interface IFooRepository
    /// </summary>
    public interface IFooRepository
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        Task<IResult> InsertAsync(FooModel model);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        Task<IResult> UpdateAsync(FooModel model);

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        Task<IResult> DeleteAsync(Guid id);

        /// <summary>
        /// 以 FooId 確認資料是否存在
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        Task<bool> IsExistsAsync(Guid id);

        /// <summary>
        /// 取得資料總數
        /// </summary>
        /// <param name="displayAll">if set to <c>true</c> [display all].</param>
        /// <returns>
        /// System.Int32.
        /// </returns>
        Task<int> GetTotalCountAsync(bool displayAll = false);

        /// <summary>
        /// 以 Id 取得資料
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Foo.</returns>
        Task<FooModel> GetAsync(Guid id);

        /// <summary>
        /// 取得指定範圍與數量的資料
        /// </summary>
        /// <param name="from">The from.</param>
        /// <param name="size">The size.</param>
        /// <param name="displayAll">if set to <c>true</c> [display all].</param>
        /// <returns>
        /// List&lt;Foo&gt;.
        /// </returns>
        Task<List<FooModel>> GetCollectionAsync(int from, int size, bool displayAll = false);
    }
}