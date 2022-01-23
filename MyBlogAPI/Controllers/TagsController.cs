using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DbAccess.Data.Models.Permission;
using DbAccess.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using MyBlogAPI.DTO.Post;
using MyBlogAPI.DTO.Tag;
using MyBlogAPI.Filters;
using MyBlogAPI.Filters.Tag;
using MyBlogAPI.Responses;
using MyBlogAPI.Services.PostService;
using MyBlogAPI.Services.TagService;

namespace MyBlogAPI.Controllers
{
    /// <summary>
    /// Controller used to expose Tag resources of the API.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;
        private readonly IPostService _postService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagsController"/> class.
        /// </summary>
        /// <param name="tagService"></param>
        /// <param name="postService"></param>
        public TagsController(ITagService tagService, IPostService postService)
        {
            _tagService = tagService;
            _postService = postService;
        }

        /// <summary>
        /// Get list of tags.
        /// </summary>
        /// <remarks>
        /// Get list of tags. The endpoint uses pagination and sort. Filter(s) can be applied for research.
        /// </remarks>
        /// <param name="sortingDirection"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet()]
        [AllowAnonymous]
        [Attributes.PermissionRequired(PermissionAction.CanRead, PermissionTarget.Tag)]
        [ProducesResponseType(typeof(PagedBlogResponse<GetTagDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTags(string sortingDirection = "ASC", int page = 1,
            int size = 10, string name = null)
        {
            var validPagination = new PaginationFilter(page, size);
            var filterSpecification = new TagQueryFilter(name).GetFilterSpecification();
            var data = await _tagService.GetTags(filterSpecification,
                new PagingSpecification((validPagination.Page - 1) * validPagination.Limit, validPagination.Limit),
                new SortTagFilter(sortingDirection).GetSorting());

            return Ok(new PagedBlogResponse<GetTagDto>(data, validPagination.Page, validPagination.Limit,
                await _tagService.CountTagsWhere(filterSpecification)));
        }

        /// <summary>
        /// Get a tag by giving its Id.
        /// </summary>
        /// <remarks>
        /// Get a tag by giving its Id.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        [Attributes.PermissionRequired(PermissionAction.CanRead, PermissionTarget.Tag)]
        [ProducesResponseType(typeof(GetTagDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _tagService.GetTag(id));
        }

        /// <summary>
        /// Add a tag.
        /// </summary>
        /// <remarks>
        /// Add a tag.
        /// </remarks>
        [HttpPost]
        [Attributes.PermissionRequired(PermissionAction.CanCreate, PermissionTarget.Tag)]
        [ProducesResponseType(typeof(GetTagDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddTag(AddTagDto user)
        {
            return Ok(await _tagService.AddTag(user));
        }

        /// <summary>
        /// Update a tag.
        /// </summary>
        /// <remarks>
        /// Update a tag.
        /// </remarks>
        /// <param name="tag"></param>
        /// <returns></returns>
        [HttpPut]
        [Attributes.PermissionRequired(PermissionAction.CanUpdate, PermissionTarget.Tag)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateTag(UpdateTagDto tag)
        {
            if (await _tagService.GetTag(tag.Id) == null)
                return NotFound();
            await _tagService.UpdateTag(tag);
            return Ok();
        }

        /// <summary>
        /// Delete a tag by giving its id.
        /// </summary>
        /// <remarks>
        /// Delete a tag by giving its id.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        [Attributes.PermissionRequired(PermissionAction.CanDelete, PermissionTarget.Tag)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (await _tagService.GetTag(id) == null)
                return NotFound();
            await _tagService.DeleteTag(id);
            return Ok();
        }

        /// <summary>
        /// Get posts from a tag by giving tag's id.
        /// </summary>
        /// <remarks>
        /// Get posts from a tag by giving tag's id.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}/Posts/")]
        [AllowAnonymous]
        [Attributes.PermissionRequired(PermissionAction.CanRead, PermissionTarget.Tag)]
        [ProducesResponseType(typeof(IEnumerable<GetPostDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPostsFromTag(int id)
        {
            return Ok(await _postService.GetPostsFromTag(id));
        }
    }
}
