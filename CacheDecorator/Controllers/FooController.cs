using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CacheDecorator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CacheDecorator.Controllers
{
    /// <summary>
    /// Class FooController.
    /// Implements the <see cref="Microsoft.AspNetCore.Mvc.Controller" />
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    public class FooController : Controller
    {
        protected const int PageSize = 10;

        /// <summary>
        /// Initializes a new instance of the <see cref="FooController"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="fooService">The foo service.</param>
        public FooController(IMapper mapper, IFooService fooService)
        {
            this._mapper = mapper;
            this.FooService = fooService;
        }

        private readonly IMapper _mapper;

        private IFooService FooService { get; set; }

        //-----------------------------------------------------------------------------------------

        /// <summary>
        /// Indexe.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [CoreProfiling]
        public async Task<IActionResult> Index(int page = 1)
        {
            var totalCount = await this.FooService.GetTotalCountAsync();

            var from = totalCount.Equals(0)
                ? 1
                : (page - 1) * PageSize + 1;

            var fooCollection = await this.FooService.GetCollectionAsync(from, PageSize, true);

            var viewModel = new FooListViewModel
            {
                FooViewModels = PagingHelper.GetPagedResult<FooViewModel>
                (
                    page,
                    PageSize,
                    totalCount,
                    this._mapper.Map<List<FooObject>, List<FooViewModel>>(fooCollection)
                )
            };

            return View(viewModel);
        }

        /// <summary>
        /// 明細
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [CoreProfiling]
        public async Task<IActionResult> Details(Guid id)
        {
            if (id.Equals(Guid.Empty))
            {
                return this.RedirectToAction("Index", "Foo");
            }

            var fooObject = await this.FooService.GetAsync(id);
            if (fooObject.EqualNull())
            {
                return this.RedirectToAction("Index", "Foo");
            }

            var viewModel = this._mapper.Map<FooObject, FooViewModel>(fooObject);
            return this.View(viewModel);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <returns>IActionResult.</returns>
        [CoreProfiling]
        public IActionResult Create()
        {
            var viewModel = new FooCreateViewModel();
            return this.View(viewModel);
        }

        /// <summary>
        /// 新增 (POST)
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [CoreProfiling]
        [HttpPost]
        public async Task<IActionResult> Create(FooCreateViewModel model)
        {
            if (this.ModelState.IsValid.Equals(false))
            {
                var viewModel = new FooCreateViewModel();
                return this.View(viewModel);
            }

            var fooObject = new FooObject
            {
                FooId = Guid.NewGuid(),
                Name = model.Name,
                Description = model.Description,
                Enable = model.Enable,
                CreateTime = SystemTime.UtcNow,
                UpdateTime = SystemTime.UtcNow
            };

            await this.FooService.InsertAsync(fooObject);

            return this.RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// 編輯
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [CoreProfiling]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id.Equals(Guid.Empty))
            {
                return this.RedirectToAction(nameof(this.Index));
            }

            var fooObject = await this.FooService.GetAsync(id);

            if (fooObject.EqualNull())
            {
                return this.RedirectToAction(nameof(this.Index));
            }

            var viewModel = this._mapper.Map<FooObject, FooUpdateViewModel>(fooObject);

            return this.View(viewModel);
        }

        /// <summary>
        /// 編輯 (POST)
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [CoreProfiling]
        [HttpPost]
        public async Task<IActionResult> Edit(FooUpdateViewModel model)
        {
            var fooObject = await this.FooService.GetAsync(model.FooId);

            if (fooObject.EqualNull())
            {
                return this.RedirectToAction(nameof(this.Index));
            }

            fooObject.Name = model.Name;
            fooObject.Description = model.Description;
            fooObject.Enable = model.Enable;
            fooObject.UpdateTime = SystemTime.UtcNow;

            await this.FooService.UpdateAsync(fooObject);

            return this.RedirectToAction(nameof(this.Index));
        }

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [CoreProfiling]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id.Equals(Guid.Empty))
            {
                return this.RedirectToAction(nameof(this.Index));
            }

            var fooObject = await this.FooService.GetAsync(id);
            if (fooObject.EqualNull())
            {
                return this.RedirectToAction(nameof(this.Index));
            }

            var viewModel = this._mapper.Map<FooObject, FooViewModel>(fooObject);
            return this.View(viewModel);
        }

        /// <summary>
        /// 刪除 (POST)
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="collection">The collection.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [CoreProfiling]
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id, IFormCollection collection)
        {
            var fooObject = await this.FooService.GetAsync(id);
            if (fooObject.EqualNull())
            {
                return this.RedirectToAction(nameof(this.Index));
            }

            try
            {
                await this.FooService.DeleteAsync(id);

                return this.RedirectToAction(nameof(this.Index));
            }
            catch
            {
                var viewModel = this._mapper.Map<FooObject, FooViewModel>(fooObject);
                return this.View(viewModel);
            }
        }
    }
}