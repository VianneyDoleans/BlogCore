using System.Collections.Generic;
using System.Threading.Tasks;
using BlogCoreAPI.Authorization.Permissions;
using BlogCoreAPI.Builders.Specifications;
using BlogCoreAPI.Builders.Specifications.User;
using BlogCoreAPI.DTOs.Comment;
using BlogCoreAPI.DTOs.Like;
using BlogCoreAPI.DTOs.User;
using BlogCoreAPI.Models.DTOs.Post;
using BlogCoreAPI.Models.DTOs.User;
using BlogCoreAPI.Models.Queries;
using BlogCoreAPI.Responses;
using BlogCoreAPI.Services.CommentService;
using BlogCoreAPI.Services.LikeService;
using BlogCoreAPI.Services.PostService;
using BlogCoreAPI.Services.RoleService;
using BlogCoreAPI.Services.UserService;
using DBAccess.Data.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogCoreAPI.Controllers
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
        private readonly IAuthorizationService _authorizationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="likeService"></param>
        /// <param name="postService"></param>
        /// <param name="commentService"></param>
        /// <param name="roleService"></param>
        /// <param name="authorizationService"></param>
        public UsersController(IUserService userService, ILikeService likeService, IPostService postService, 
            ICommentService commentService, IRoleService roleService, IAuthorizationService authorizationService)
        {
            _userService = userService;
            _likeService = likeService;
            _postService = postService;
            _commentService = commentService;
            _roleService = roleService;
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Get list of users.
        /// </summary>
        /// <remarks>
        /// Get list of users. The endpoint uses pagination and sort. Filter(s) can be applied for research.
        /// </remarks>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PagedBlogResponse<GetUserDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsers([FromQuery] GetUserQueryParameters parameters)
        {
            parameters ??= new GetUserQueryParameters();

            var pagingSpecificationBuilder = new PagingSpecificationBuilder(parameters.Page, parameters.PageSize);
            var filterSpecification = new UserFilterSpecificationBuilder()
                .WithInUserName(parameters.InUserName)
                .WithFromLastLogin(parameters.FromLastLoginDate)
                .WithToLastLogin(parameters.ToLastLoginDate)
                .WithFromRegister(parameters.FromRegistrationDate)
                .WithToRegister(parameters.ToRegistrationDate)
                .Build();
            var data = await _userService.GetUsers(filterSpecification,
                pagingSpecificationBuilder.Build(), new UserSortSpecificationBuilder(parameters.OrderBy, parameters.SortBy).Build());

            return Ok(new PagedBlogResponse<GetUserDto>(data, pagingSpecificationBuilder.Page, pagingSpecificationBuilder.Limit,
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
        [ProducesResponseType(typeof(GetUserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _userService.GetUser(id));
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddRoleToUser(int id, int roleId)
        {
            if (await _userService.GetUser(id) == null)
                return NotFound("Doesn't found the user.");
            if (await _roleService.GetRole(roleId) == null)
                return NotFound("Doesn't found the role.");

            var roleEntity = await _roleService.GetRoleEntity(roleId);
            var roleAuthorized = await _authorizationService.AuthorizeAsync(User, roleEntity, new PermissionRequirement(PermissionAction.CanUpdate, PermissionTarget.Role));
            if (!roleAuthorized.Succeeded)
                return Forbid();

            var userEntity = await _userService.GetUserEntity(id);
            var userAuthorized = await _authorizationService.AuthorizeAsync(User, userEntity, new PermissionRequirement(PermissionAction.CanUpdate, PermissionTarget.User));
            if (!userAuthorized.Succeeded)
                return Forbid();

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> RemoveRoleToUser(int id, int roleId)
        {
            if (await _userService.GetUser(id) == null)
                return NotFound("Doesn't found the user.");
            if (await _roleService.GetRole(roleId) == null)
                return NotFound("Doesn't found the role.");

            var roleEntity = await _roleService.GetRoleEntity(roleId);
            var roleAuthorized = await _authorizationService.AuthorizeAsync(User, roleEntity, new PermissionRequirement(PermissionAction.CanUpdate, PermissionTarget.Role));
            if (!roleAuthorized.Succeeded)
                return Forbid();

            var userEntity = await _userService.GetUserEntity(id);
            var userAuthorized = await _authorizationService.AuthorizeAsync(User, userEntity, new PermissionRequirement(PermissionAction.CanUpdate, PermissionTarget.User));
            if (!userAuthorized.Succeeded)
                return Forbid();

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateUser(UpdateUserDto user)
        {
            if (await _userService.GetUser(user.Id) == null)
                return NotFound();

            var userEntity = await _userService.GetUserEntity(user.Id);
            var authorized = await _authorizationService.AuthorizeAsync(User, userEntity, new PermissionRequirement(PermissionAction.CanUpdate, PermissionTarget.User));
            if (!authorized.Succeeded)
                return Forbid();

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (await _userService.GetUser(id) == null)
                return NotFound();

            var userEntity = await _userService.GetUserEntity(id);
            var authorized = await _authorizationService.AuthorizeAsync(User, userEntity, new PermissionRequirement(PermissionAction.CanDelete, PermissionTarget.User));
            if (!authorized.Succeeded)
                return Forbid();

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
        [ProducesResponseType(typeof(IEnumerable<GetLikeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLikesFromUser(int id)
        {
            return Ok(await _likeService.GetLikesFromUser(id));
        }
    }
}
