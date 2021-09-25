using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DbAccess.Specifications;
using MyBlogAPI.DTO.Tag;
using MyBlogAPI.Filters;
using MyBlogAPI.Filters.Tag;
using MyBlogAPI.Services.PostService;
using MyBlogAPI.Services.TagService;

namespace MyBlogAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;
        private readonly IPostService _postService;

        public TagsController(ITagService tagService, IPostService postService)
        {
            _tagService = tagService;
            _postService = postService;
        }

        [HttpGet("All")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _tagService.GetAllTags());
        }

        [HttpGet()]
        public async Task<IActionResult> GetTags(string sortingDirection = "ASC", int pageNumber = 1,
            int pageSize = 10, string name = null)
        {
            var validFilter = new PaginationFilter(pageNumber, pageSize);

            return Ok(await _tagService.GetTags(new TagQueryFilter(name).GetFilterSpecification(),
                new PagingSpecification((validFilter.PageNumber - 1) * validFilter.PageSize, validFilter.PageSize),
                new SortTagFilter(sortingDirection).GetSorting()));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _tagService.GetTag(id));
        }

        [HttpGet("{id:int}/Posts/")]
        public async Task<IActionResult> GetPostsFromTag(int id)
        {
            return Ok(await _postService.GetPostsFromTag(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddTag(AddTagDto user)
        {
            return Ok(await _tagService.AddTag(user));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTag(UpdateTagDto tag)
        {
            if (await _tagService.GetTag(tag.Id) == null)
                return NotFound();
            await _tagService.UpdateTag(tag);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (await _tagService.GetTag(id) == null)
                return NotFound();
            await _tagService.DeleteTag(id);
            return Ok();
        }
    }
}
