using System.Collections.Generic;
using System.Threading.Tasks;
using BlogCoreAPI.Authorization.Attributes;
using BlogCoreAPI.Builders.Specifications;
using BlogCoreAPI.Builders.Specifications.Category;
using BlogCoreAPI.DTOs.Category;
using BlogCoreAPI.Models;
using BlogCoreAPI.Models.DTOs.Category;
using BlogCoreAPI.Models.DTOs.Post;
using BlogCoreAPI.Models.Sort;
using BlogCoreAPI.Responses;
using BlogCoreAPI.Services.CategoryService;
using BlogCoreAPI.Services.PostService;
using DBAccess.Data.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogCoreAPI.Controllers
{
    /// <summary>
    /// Controller used to expose Category resources of the API.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IPostService _postService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoriesController"/> class.
        /// </summary>
        /// <param name="categoryService"></param>
        /// <param name="postService"></param>
        public CategoriesController(ICategoryService categoryService, IPostService postService)
        {
            _categoryService = categoryService;
            _postService = postService;
        }

        /// <summary>
        /// Get list of categories.
        /// </summary>
        /// <remarks>
        /// Get list of categories. The endpoint uses pagination and sort. Filter(s) can be applied for research.
        /// </remarks>
        /// <param name="order"></param>
        /// <param name="categorySort"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="name"></param>
        /// <param name="minimumPostNumber"></param>
        /// <param name="maximumPostNumber"></param>
        /// <returns></returns>
        [HttpGet()]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PagedBlogResponse<GetCategoryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategories(Order order = Order.Asc, CategorySort categorySort = CategorySort.Name, int page = 1, int size = 10, 
            string name = null, int? minimumPostNumber = null, int? maximumPostNumber = null)
        {
            var pagingSpecificationBuilder = new PagingSpecificationBuilder(page, size);

            var filterSpecification = new CategoryFilterSpecificationBuilder(name, minimumPostNumber, maximumPostNumber).Build();
            var data = await _categoryService.GetCategories(filterSpecification,
                pagingSpecificationBuilder.Build(), new CategorySortSpecificationBuilder(order, categorySort).Build());

            return Ok(new PagedBlogResponse<GetCategoryDto>(data, pagingSpecificationBuilder.Page, pagingSpecificationBuilder.Limit, 
                await _categoryService.CountCategoriesWhere(filterSpecification)));
        }

        /// <summary>
        /// Get a category by giving its Id.
        /// </summary>
        /// <remarks>
        /// Get a category by giving its Id.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetCategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _categoryService.GetCategory(id));
        }

        /// <summary>
        /// Add a category.
        /// </summary>
        /// <remarks>
        /// Add a category.
        /// </remarks>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionWithPermissionRangeAllRequired(PermissionAction.CanCreate, PermissionTarget.Category)]
        [ProducesResponseType(typeof(GetCategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddCategory(AddCategoryDto user)
        {
            return Ok(await _categoryService.AddCategory(user));
        }

        /// <summary>
        /// Update a category.
        /// </summary>
        /// <remarks>
        /// Update a category.
        /// </remarks>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [PermissionWithPermissionRangeAllRequired(PermissionAction.CanUpdate, PermissionTarget.Category)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDto category)
        {
            if (await _categoryService.GetCategory(category.Id) == null)
                return NotFound();
            await _categoryService.UpdateCategory(category);
            return Ok();
        }

        /// <summary>
        /// Delete a category by giving its id.
        /// </summary>
        /// <remarks>
        /// Delete a category by giving its id.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        [PermissionWithPermissionRangeAllRequired(PermissionAction.CanDelete, PermissionTarget.Category)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (await _categoryService.GetCategory(id) == null)
                return NotFound();
            await _categoryService.DeleteCategory(id);
            return Ok();
        }

        /// <summary>
        /// Get posts from a category by giving category's id.
        /// </summary>
        /// <remarks>
        /// Get posts from a category by giving category's id.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}/Posts")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<GetPostDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPostFromCategory(int id)
        {
            return Ok(await _postService.GetPostsFromCategory(id));
        }
    }
}
