using CacheDecorator.Infrastructure.Paging;
using System;

namespace CacheDecorator.Models
{
    /// <summary>
    /// Class FooListViewModel.
    /// </summary>
    public class FooListViewModel
    {
        /// <summary>
        /// The Foos.
        /// </summary>
        public PagedResult<FooViewModel> FooViewModels { get; set; }
    }
}