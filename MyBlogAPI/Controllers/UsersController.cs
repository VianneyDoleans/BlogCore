using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DbAccess.Data.POCO.Permission;
using DbAccess.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlogAPI.DTO.Comment;
using MyBlogAPI.DTO.Like;
using MyBlogAPI.DTO.Post;
using MyBlogAPI.DTO.User;
using MyBlogAPI.Filters;
using MyBlogAPI.Filters.User;
using MyBlogAPI.Responses;
using MyBlogAPI.Services.CommentService;
using MyBlogAPI.Services.LikeService;
using MyBlogAPI.Services.PostService;
using MyBlogAPI.Services.RoleService;
using MyBlogAPI.Services.UserService;

namespace MyBlogAPI.Controllers
{
    /// <summary>
    /// Controller used to expose User resources of the API.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly ILikeService _likeService;
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="likeService"></param>
        /// <param name="postService"></param>
        /// <param name="commentService"></param>
        /// <param name="roleService"></param>
        public UsersController(IUserService userService, ILikeService likeService, IPostService postService, 
            ICommentService commentService, IRoleService roleService)
        {
            _userService = userService;
            _likeService = likeService;
            _postService = postService;
            _commentService = commentService;
            _roleService = roleService;
        }

        /// <summary>
        /// Get list of users.
        /// </summary>
        /// <remarks>
        /// Get list of users. The endpoint uses pagination and sort. Filter(s) can be applied for research.
        /// </remarks>
        /// <param name="sortingDirection"></param>
        /// <param name="orderBy"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="name"></param>
        /// <param name="registerBefore"></param>
        /// <param name="lastLoginBefore"></param>
        /// <returns></returns>
        [HttpGet()]
        [AllowAnonymous]
        [Attributes.PermissionRequired(PermissionAction.CanRead, PermissionTarget.User)]
        [ProducesResponseType(typeof(PagedBlogResponse<GetUserDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsers(string sortingDirection = "ASC", string orderBy = null, int page = 1,
            int size = 10, string name = null, DateTime? registerBefore = null, DateTime? lastLoginBefore = null)
        {
            var validPagination = new PaginationFilter(page, size);
            var filterSpecification = new UserQueryFilter(name, lastLoginBefore, registerBefore).GetFilterSpecification();
            var data = await _userService.GetUsers(filterSpecification,
                new PagingSpecification((validPagination.Page - 1) * validPagination.Limit, validPagination.Limit),
                new SortUserFilter(sortingDirection, orderBy).GetSorting());

            return Ok(new PagedBlogResponse<GetUserDto>(data, validPagination.Page, validPagination.Limit,
                await _userService.CountUsersWhere(filterSpecification)));
        }

        /// <summary>
        /// Get a user by giving its Id.
        /// </summary>
        /// <remarks>
        /// Get a user by giving its Id.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        [Attributes.PermissionRequired(PermissionAction.CanRead, PermissionTarget.User)]
        [ProducesResponseType(typeof(GetUserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _userService.GetUser(id));
        }

        /// <summary>
        /// Create a user.
        /// </summary>
        /// <remarks>
        /// Create a user.
        /// </remarks>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("SignUp")]
        [AllowAnonymous]
        [Attributes.PermissionRequired(PermissionAction.CanCreate, PermissionTarget.User)]
        [ProducesResponseType(typeof(GetUserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> SignUp(AddUserDto user)
        {
            return Ok(await _userService.AddUser(user));
        }

        /// <summary>
        /// Sign In as a user.
        /// </summary>
        /// <remarks>
        /// Sign In as a user.
        /// </remarks>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        [HttpPost("SignIn")]
        [AllowAnonymous]
        [Attributes.PermissionRequired(PermissionAction.CanCreate, PermissionTarget.User)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignIn(UserLoginDto userLogin)
        {
            if (await _userService.SignIn(userLogin))
                return Ok();
            return BadRequest("Bad username or password.");
        }

        /// <summary>
        /// Add a role to a user.
        /// </summary>
        /// <remarks>
        /// Add a role to a user.
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPost("{id:int}/Role/{roleId:int}")]
        [Attributes.PermissionRequired(PermissionAction.CanUpdate, PermissionTarget.User)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddRoleToUser(int id, int roleId)
        {
            if (await _userService.GetUser(id) == null)
                return NotFound("Doesn't found the user.");
            if (await _roleService.GetRole(roleId) == null)
                return NotFound("Doesn't found the role.");
            await _userService.AddUserRole(new UserRoleDto() {UserId = id, RoleId = roleId});
            return Ok();
        }

        /// <summary>
        /// Remove a role to a user.
        /// </summary>
        /// <remarks>
        /// Remove a role to a user.
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}/Role/{roleId:int}")]
        [Attributes.PermissionRequired(PermissionAction.CanUpdate, PermissionTarget.User)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> RemoveRoleToUser(int id, int roleId)
        {
            if (await _userService.GetUser(id) == null)
                return NotFound("Doesn't found the user.");
            if (await _roleService.GetRole(roleId) == null)
                return NotFound("Doesn't found the role.");
            await _userService.RemoveUserRole(new UserRoleDto {UserId = id, RoleId = roleId});
            return Ok();
        }

        /// <summary>
        /// Update a user.
        /// </summary>
        /// <remarks>
        /// Update a user.
        /// </remarks>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut]
        [Attributes.PermissionRequired(PermissionAction.CanUpdate, PermissionTarget.User)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateUser(UpdateUserDto user)
        {
            if (await _userService.GetUser(user.Id) == null)
                return NotFound();
            await _userService.UpdateUser(user);
            return Ok();
        }

        /// <summary>
        /// Delete a user by giving its id.
        /// </summary>
        /// <remarks>
        /// Delete a user by giving its id.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        [Attributes.PermissionRequired(PermissionAction.CanDelete, PermissionTarget.User)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (await _userService.GetUser(id) == null)
                return NotFound();
            await _userService.DeleteUser(id);
            return Ok();
        }

        /// <summary>
        /// Get posts written by a user by giving user's id.
        /// </summary>
        /// <remarks>
        /// Get posts written by a user by giving user's id.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}/Posts/")]
        [AllowAnonymous]
        [Attributes.PermissionRequired(PermissionAction.CanRead, PermissionTarget.User)]
        [ProducesResponseType(typeof(IEnumerable<GetPostDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPostsFromUser(int id)
        {
            return Ok(await _postService.GetPostsFromUser(id));
        }

        /// <summary>
        /// Get comments written by a user by giving user's id.
        /// </summary>
        /// <remarks>
        /// Get comments written by a user by giving user's id.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}/Comments/")]
        [AllowAnonymous]
        [Attributes.PermissionRequired(PermissionAction.CanRead, PermissionTarget.User)]
        [ProducesResponseType(typeof(IEnumerable<GetCommentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCommentsFromUser(int id)
        {
            return Ok(await _commentService.GetCommentsFromUser(id));
        }

        /// <summary>
        /// Get likes given by a user by giving user's id.
        /// </summary>
        /// <remarks>
        /// Get likes given by a user by giving user's id.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}/Likes/")]
        [AllowAnonymous]
        [Attributes.PermissionRequired(PermissionAction.CanRead, PermissionTarget.User)]
        [ProducesResponseType(typeof(IEnumerable<GetLikeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLikesFromUser(int id)
        {
            return Ok(await _likeService.GetLikesFromUser(id));
        }
    }
}
