using System;
using System.Collections.Generic;
using System.Linq;

namespace CacheDecorator.Infrastructure.Paging
{
    /// <summary>
    /// Class PagingHelper.
    /// </summary>
    public class PagingHelper
    {
        /// <summary>
        /// Gets the PagedResult.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="total">The total.</param>
        /// <param name="collection">The collection.</param>
        /// <returns>PagedResult&lt;T&gt;.</returns>
        public static PagedResult<T> GetPagedResult<T>(int page, int pageSize, int total, IEnumerable<T> collection)
        {
            var result = new PagedResult<T>
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = total,
                Results = collection.ToList()
            };

            var pageCount = (double)result.TotalCount / pageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            return result;
        }
    }
}