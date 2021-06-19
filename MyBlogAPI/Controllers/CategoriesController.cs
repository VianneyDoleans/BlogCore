using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MyBlogAPI.DTO.Category;
using MyBlogAPI.Services.CategoryService;
using MyBlogAPI.Services.PostService;

namespace MyBlogAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IPostService _postService;

        public CategoriesController(ICategoryService categoryService, IPostService postService)
        {
            _categoryService = categoryService;
            _postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _categoryService.GetAllCategories());
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _categoryService.GetCategory());
        }

        [HttpGet("{id}")]
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (await _categoryService.GetCategory(id) == null)
                return NotFound();
            await _categoryService.DeleteCategory(id);
            return Ok();
        }

        [HttpGet("{id}/Posts")]
        public async Task<IActionResult> GetPostFromCategory(int id)
        {
            return Ok(await _postService.GetPostsFromCategory(id));
        }
    }
}
