using System.Collections.Generic;

namespace CacheDecorator.Infrastructure.Paging
{
    /// <summary>
    /// Class PagedResult.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="PagedResultBase" />
    public class PagedResult<T> : PagedResultBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedResult{T}"/> class.
        /// </summary>
        public PagedResult()
        {
            this.Results = new List<T>();
        }

        /// <summary>
        /// the results.
        /// </summary>
        public IList<T> Results { get; set; }
    }
}