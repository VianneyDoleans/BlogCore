using System.Threading.Tasks;
using BlogCoreAPI.Authorization.Permissions;
using BlogCoreAPI.Builders.Specifications;
using BlogCoreAPI.Builders.Specifications.Like;
using BlogCoreAPI.Models.DTOs.Like;
using BlogCoreAPI.Models.Queries;
using BlogCoreAPI.Responses;
using BlogCoreAPI.Services.LikeService;
using DBAccess.Data.Permission;
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
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PagedBlogResponse<GetLikeDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLikes([FromQuery] GetLikeQueryParameters parameters)
        {
            var pagingSpecificationBuilder = new PagingSpecificationBuilder(parameters.Page, parameters.PageSize);
            var filterSpecification = new LikeFilterSpecificationBuilder(parameters.LikeableType).Build();
            var data = await _likeService.GetLikes(filterSpecification,
                pagingSpecificationBuilder.Build(), new SortLikeBuilder(parameters.OrderBy).Build());

            return Ok(new PagedBlogResponse<GetLikeDto>(data, pagingSpecificationBuilder.Page, pagingSpecificationBuilder.Limit,
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
