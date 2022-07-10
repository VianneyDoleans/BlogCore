using System.Collections.Generic;
using System.Threading.Tasks;
using BlogCoreAPI.Authorization.Permissions;
using BlogCoreAPI.Builders.Specifications;
using BlogCoreAPI.Builders.Specifications.Comment;
using BlogCoreAPI.DTOs.Comment;
using BlogCoreAPI.DTOs.Like;
using BlogCoreAPI.Models.Queries;
using BlogCoreAPI.Responses;
using BlogCoreAPI.Services.CommentService;
using BlogCoreAPI.Services.LikeService;
using DBAccess.Data.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogCoreAPI.Controllers
{
    /// <summary>
    /// Controller used to expose Comment resources of the API.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly ILikeService _likeService;
        private readonly IAuthorizationService _authorizationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentsController"/> class.
        /// </summary>
        /// <param name="commentService"></param>
        /// <param name="likeService"></param>
        /// <param name="authorizationService"></param>
        public CommentsController(ICommentService commentService, ILikeService likeService, IAuthorizationService authorizationService)
        {
            _commentService = commentService;
            _likeService = likeService;
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Get list of comments.
        /// </summary>
        /// <remarks>
        /// Get list of comments. The endpoint uses pagination and sort. Filter(s) can be applied for research.
        /// </remarks>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PagedBlogResponse<GetCommentDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetComments([FromQuery]GetCommentQueryParameters parameters)
        {
            parameters ??= new GetCommentQueryParameters();

            var pagingSpecificationBuilder = new PagingSpecificationBuilder(parameters.Page, parameters.PageSize);
            var filterSpecification = new CommentFilterSpecificationBuilder()
                .WithPostParentName(parameters.InPostParentName)
                .WithInContent(parameters.InContent)
                .WithInAuthorUserName(parameters.InAuthorUserName)
                .Build();
            var data = await _commentService.GetComments(filterSpecification,
                pagingSpecificationBuilder.Build(), new CommentSortSpecificationBuilder(parameters.Order, parameters.Sort).Build());

            return Ok(new PagedBlogResponse<GetCommentDto>(data, pagingSpecificationBuilder.Page, pagingSpecificationBuilder.Limit,
                await _commentService.CountCommentsWhere(filterSpecification)));
        }

        /// <summary>
        /// Get a comment by giving its Id.
        /// </summary>
        /// <remarks>
        /// Get a comment by giving its Id.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetCommentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _commentService.GetComment(id));
        }

        /// <summary>
        /// Add a comment.
        /// </summary>
        /// <remarks>
        /// Add a comment.
        /// </remarks>
        /// <param name="comment"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(GetCommentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddComment(AddCommentDto comment)
        {
            var authorized = await _authorizationService.AuthorizeAsync(User, comment, new PermissionRequirement(PermissionAction.CanUpdate, PermissionTarget.Comment));
            if (!authorized.Succeeded)
                return Forbid();

            return Ok(await _commentService.AddComment(comment));
        }

        /// <summary>
        /// Update a comment.
        /// </summary>
        /// <remarks>
        /// Update a comment.
        /// </remarks>
        /// <param name="comment"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateComment(UpdateCommentDto comment)
        {
            var authorized = await  _authorizationService.AuthorizeAsync(User, comment, new PermissionRequirement(PermissionAction.CanUpdate, PermissionTarget.Comment));
            if (!authorized.Succeeded)
                return Forbid();

            await _commentService.UpdateComment(comment);
            return Ok();
        }

        /// <summary>
        /// Delete a comment by giving its id.
        /// </summary>
        /// <remarks>
        /// Delete a comment by giving its id.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var commentDto = await _commentService.GetComment(id);
            var authorized = await _authorizationService.AuthorizeAsync(User, commentDto, new PermissionRequirement(PermissionAction.CanUpdate, PermissionTarget.Comment));
            if (!authorized.Succeeded)
                return Forbid();

            await _commentService.DeleteComment(id);
            return Ok();
        }

        /// <summary>
        /// Get likes from a comment by giving comment's id.
        /// </summary>
        /// <remarks>
        /// Get likes from a comment by giving comment's id.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}/Likes/")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<GetLikeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLikesFromComment(int id)
        {
            return Ok(await _likeService.GetLikesFromComment(id));
        }
    }
}
