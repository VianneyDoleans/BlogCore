using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DbAccess.Specifications;
using Microsoft.AspNetCore.Http;
using MyBlogAPI.DTO.Category;
using MyBlogAPI.Filters;
using MyBlogAPI.Filters.Category;
using MyBlogAPI.Responses;
using MyBlogAPI.Services.CategoryService;
using MyBlogAPI.Services.PostService;

namespace MyBlogAPI.Controllers
{
    /// <summary>
    /// Controller used to expose Category resources of the API.
    /// </summary>
    [ApiController]
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
        /// 
        /// </summary>
        /// <remarks></remarks>
        /// <returns></returns>
        [HttpGet("All")]
        [ProducesResponseType(typeof(IEnumerable<GetCategoryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(new BlogResponse<GetCategoryDto>(await _categoryService.GetAllCategories()));
        }

        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<GetCategoryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategories(string sortingDirection = "ASC", int offset = 1, int limit = 10, 
            string name = null, int? minimumPostNumber = null, int? maximumPostNumber = null)
        {
            var validFilter = new PaginationFilter(offset, limit);

            return Ok(new PagedBlogResponse<GetCategoryDto>(await _categoryService.GetCategories(new CategoryQueryFilter(name, minimumPostNumber, maximumPostNumber).GetFilterSpecification(),
                new PagingSpecification((validFilter.Offset - 1) * validFilter.Limit, validFilter.Limit),
                new SortCategoryFilter(sortingDirection).GetSorting(), validFilter.Offset, validFilter.Limit, _categoryService)));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _categoryService.GetCategory(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddCategoryDto user)
        {
            return Ok(await _categoryService.AddCategory(user));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateCategoryDto category)
        {
            if (await _categoryService.GetCategory(category.Id) == null)
                return NotFound();
            await _categoryService.UpdateCategory(category);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (await _categoryService.GetCategory(id) == null)
                return NotFound();
            await _categoryService.DeleteCategory(id);
            return Ok();
        }

        [HttpGet("{id:int}/Posts")]
        public async Task<IActionResult> GetPostFromCategory(int id)
        {
            return Ok(await _postService.GetPostsFromCategory(id));
        }
    }
}
