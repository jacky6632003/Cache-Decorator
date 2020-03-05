using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CacheDecorator.Components
{
    /// <summary>
    /// Class PagerViewComponent.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ViewComponent" />
    public class PagerViewComponent : ViewComponent
    {
        /// <summary>
        /// invoke as an asynchronous operation.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>Task&lt;IViewComponentResult&gt;.</returns>
        public async Task<IViewComponentResult> InvokeAsync(PagedResultBase result)
        {
            result.LinkTemplate = this.Url.Action
            (
                this.RouteData.Values["action"].ToString(),
                new { page = "{0}" }
            );

            return this.View("Default", result);
        }
    }
}