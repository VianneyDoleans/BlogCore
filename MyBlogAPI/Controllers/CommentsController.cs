﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DbAccess.Data.POCO.Permission;
using DbAccess.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using MyBlogAPI.Attributes;
using MyBlogAPI.DTO.Comment;
using MyBlogAPI.DTO.Like;
using MyBlogAPI.Filters;
using MyBlogAPI.Filters.Comment;
using MyBlogAPI.Responses;
using MyBlogAPI.Services.CommentService;
using MyBlogAPI.Services.LikeService;

namespace MyBlogAPI.Controllers
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
        /// <param name="sortingDirection"></param>
        /// <param name="orderBy"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="authorUsername"></param>
        /// <param name="postParentName"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpGet()]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PagedBlogResponse<GetCommentDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetComments(string sortingDirection = "ASC", string orderBy = null, int page = 1, 
            int size = 10, string authorUsername = null, string postParentName = null, string content = null)
        {
            var validPagination = new PaginationFilter(page, size);
            var filterSpecification = new CommentQueryFilter(authorUsername, postParentName, content).GetFilterSpecification();
            var data = await _commentService.GetComments(filterSpecification,
                new PagingSpecification((validPagination.Page - 1) * validPagination.Limit, validPagination.Limit),
                new SortCommentFilter(sortingDirection, orderBy).GetSorting());

            return Ok(new PagedBlogResponse<GetCommentDto>(data, validPagination.Page, validPagination.Limit,
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
            var commentEntity = await _commentService.GetCommentEntity(comment.Author);
            var authorized = await _authorizationService.AuthorizeAsync(User, commentEntity, new PermissionRequirement(PermissionAction.CanUpdate, PermissionTarget.Comment));
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
            var commentDto = await _commentService.GetComment(comment.Id);
            if (commentDto == null)
                return NotFound();
            var commentEntity = await _commentService.GetCommentEntity(comment.Id);
            var authorized = await  _authorizationService.AuthorizeAsync(User, commentEntity, new PermissionRequirement(PermissionAction.CanUpdate, PermissionTarget.Comment));
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
            if (await _commentService.GetComment(id) == null)
                return NotFound();
            var commentEntity = await _commentService.GetCommentEntity(id);
            var authorized = await _authorizationService.AuthorizeAsync(User, commentEntity, new PermissionRequirement(PermissionAction.CanUpdate, PermissionTarget.Comment));
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
