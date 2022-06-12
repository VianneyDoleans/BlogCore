using System.Threading.Tasks;
using BlogCoreAPI.Authorization.Permissions;
using BlogCoreAPI.DTOs.Like;
using BlogCoreAPI.Filters;
using BlogCoreAPI.Filters.Like;
using BlogCoreAPI.Responses;
using BlogCoreAPI.Services.LikeService;
using DBAccess.Data.POCO;
using DBAccess.Data.POCO.Permission;
using DBAccess.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogCoreAPI.Controllers
{
    /// <summary>
    /// Controller used to expose Like resources of the API.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class LikesController : ControllerBase
    {
        private readonly ILikeService _likeService;
        private readonly IAuthorizationService _authorizationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LikesController"/> class.
        /// </summary>
        /// <param name="likeService"></param>
        /// <param name="authorizeService"></param>
        public LikesController(ILikeService likeService, IAuthorizationService authorizeService)
        {
            _likeService = likeService;
            _authorizationService = authorizeService;
        }

        /// <summary>
        /// Get list of likes.
        /// </summary>
        /// <remarks>
        /// Get list of likes. The endpoint uses pagination and sort. Filter(s) can be applied for research.
        /// </remarks>
        /// <param name="sortingDirection"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="likeableType"></param>
        /// <returns></returns>
        [HttpGet()]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PagedBlogResponse<GetLikeDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLikes(string sortingDirection = "ASC", int page = 1,
            int size = 10, LikeableType? likeableType = null)
        {
            var validPagination = new PaginationFilter(page, size);
            var filterSpecification = new LikeQueryFilter(likeableType).GetFilterSpecification();
            var data = await _likeService.GetLikes(filterSpecification,
                new PagingSpecification((validPagination.Page - 1) * validPagination.Limit, validPagination.Limit),
                new SortLikeFilter(sortingDirection).GetSorting());

            return Ok(new PagedBlogResponse<GetLikeDto>(data, validPagination.Page, validPagination.Limit,
                await _likeService.CountLikesWhere(filterSpecification)));
        }

        /// <summary>
        /// Get a like by giving its Id.
        /// </summary>
        /// <remarks>
        /// Get a like by giving its Id.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetLikeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _likeService.GetLike(id));
        }

        /// <summary>
        /// Add a like.
        /// </summary>
        /// <remarks>
        /// Add a like.
        /// </remarks>
        /// <param name="like"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(GetLikeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddLikes(AddLikeDto like)
        {
            var authorized = await _authorizationService.AuthorizeAsync(User, like, new PermissionRequirement(PermissionAction.CanCreate, PermissionTarget.Like));
            if (!authorized.Succeeded)
                return Forbid();

            return Ok(await _likeService.AddLike(like));
        }

        /// <summary>
        /// Update a like.
        /// </summary>
        /// <remarks>
        /// Update a like.
        /// </remarks>
        /// <param name="like"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateLike(UpdateLikeDto like)
        {
            var authorized = await _authorizationService.AuthorizeAsync(User, like, new PermissionRequirement(PermissionAction.CanUpdate, PermissionTarget.Like));
            if (!authorized.Succeeded)
                return Forbid();

            await _likeService.UpdateLike(like);
            return Ok();
        }

        /// <summary>
        /// Delete a like by giving its id.
        /// </summary>
        /// <remarks>
        /// Delete a like by giving its id.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteLike(int id)
        {
            var likeDto = await _likeService.GetLike(id);
            var authorized = await _authorizationService.AuthorizeAsync(User, likeDto, new PermissionRequirement(PermissionAction.CanDelete, PermissionTarget.Like));
            if (!authorized.Succeeded)
                return Forbid();

            await _likeService.DeleteLike(id);
            return Ok();
        }
    }
}
