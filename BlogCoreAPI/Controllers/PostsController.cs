﻿using System.Collections.Generic;
using System.Threading.Tasks;
using BlogCoreAPI.Authorization.Permissions;
using BlogCoreAPI.Builders;
using BlogCoreAPI.Builders.Specifications;
using BlogCoreAPI.Builders.Specifications.Post;
using BlogCoreAPI.DTOs.Comment;
using BlogCoreAPI.DTOs.Like;
using BlogCoreAPI.DTOs.Post;
using BlogCoreAPI.Responses;
using BlogCoreAPI.Services.CommentService;
using BlogCoreAPI.Services.LikeService;
using BlogCoreAPI.Services.PostService;
using DBAccess.Data.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogCoreAPI.Controllers
{
    /// <summary>
    /// Controller used to expose Post resources of the API.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;
        private readonly ILikeService _likeService;
        private readonly IAuthorizationService _authorizationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostsController"/> class.
        /// </summary>
        /// <param name="postService"></param>
        /// <param name="commentService"></param>
        /// <param name="likeService"></param>
        /// <param name="authorizationService"></param>
        public PostsController(IPostService postService, ICommentService commentService, ILikeService likeService, 
            IAuthorizationService authorizationService)
        {
            _postService = postService;
            _commentService = commentService;
            _likeService = likeService;
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Get list of posts.
        /// </summary>
        /// <remarks>
        /// Get list of posts. The endpoint uses pagination and sort. Filter(s) can be applied for research.
        /// </remarks>
        /// <param name="sortingDirection"></param>
        /// <param name="orderBy"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="name"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpGet()]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PagedBlogResponse<GetPostDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPosts(string sortingDirection = "ASC", string orderBy = null, int page = 1,
            int size = 10, string name = null, string content = null)
        {
            var pagingSpecificationBuilder = new PagingSpecificationBuilder(page, size);
            var filterSpecification = new PostFilterSpecificationBuilder(content, name).Build();
            var data = await _postService.GetPosts(filterSpecification,
                pagingSpecificationBuilder.Build(), new SortPostFilter(sortingDirection, orderBy).GetSorting());

            return Ok(new PagedBlogResponse<GetPostDto>(data, pagingSpecificationBuilder.Page, pagingSpecificationBuilder.Limit,
                await _postService.CountPostsWhere(filterSpecification)));
        }

        /// <summary>
        /// Get a post by giving its Id.
        /// </summary>
        /// <remarks>
        /// Get a post by giving its Id.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetPostDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _postService.GetPost(id));
        }

        /// <summary>
        /// Add a post.
        /// </summary>
        /// <remarks>
        /// Add a post.
        /// </remarks>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(GetPostDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddPost(AddPostDto post)
        {
            var authorized = await _authorizationService.AuthorizeAsync(User, post, new PermissionRequirement(PermissionAction.CanCreate, PermissionTarget.Post));
            if (!authorized.Succeeded)
                return Forbid();

            return Ok(await _postService.AddPost(post));
        }

        /// <summary>
        /// Update a post.
        /// </summary>
        /// <remarks>
        /// Update a post.
        /// </remarks>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdatePost(UpdatePostDto post)
        {
            var authorized = await _authorizationService.AuthorizeAsync(User, post, new PermissionRequirement(PermissionAction.CanUpdate, PermissionTarget.Post));
            if (!authorized.Succeeded)
                return Forbid();

            await _postService.UpdatePost(post);
            return Ok();
        }

        /// <summary>
        /// Delete a post by giving its id.
        /// </summary>
        /// <remarks>
        /// Delete a post by giving its id.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePost(int id)
        {
            var postDto = await _postService.GetPost(id);
            var authorized = await _authorizationService.AuthorizeAsync(User, postDto, new PermissionRequirement(PermissionAction.CanDelete, PermissionTarget.Post));
            if (!authorized.Succeeded)
                return Forbid();

            await _postService.DeletePost(id);
            return Ok();
        }

        /// <summary>
        /// Get comments from a post by giving post's id.
        /// </summary>
        /// <remarks>
        /// Get comments from a post by giving post's id.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}/Comments/")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<GetCommentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCommentsFromPost(int id)
        {
            return Ok(await _commentService.GetCommentsFromPost(id));
        }

        /// <summary>
        /// Get likes from a post by giving post's id.
        /// </summary>
        /// <remarks>
        /// Get likes from a post by giving post's id.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}/Likes/")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<GetLikeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLikesFromPost(int id)
        {
            return Ok(await _likeService.GetLikesFromPost(id));
        }
    }
}
