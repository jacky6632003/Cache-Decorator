using System;

namespace CacheDecorator.Infrastructure.Paging
{
    /// <summary>
    /// Class PagedResultBase.
    /// </summary>
    public class PagedResultBase
    {
        /// <summary>
        /// the first row on page.
        /// </summary>
        public int FirstRowOnPage => (this.CurrentPage - 1) * this.PageSize + 1;

        /// <summary>
        /// the last row on page.
        /// </summary>
        public int LastRowOnPage => Math.Min(this.CurrentPage * this.PageSize, this.TotalCount);

        /// <summary>
        /// the current page.
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// the page count.
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// the size of the page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// the total count.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// the link template.
        /// </summary>
        public string LinkTemplate { get; set; }
    }
}