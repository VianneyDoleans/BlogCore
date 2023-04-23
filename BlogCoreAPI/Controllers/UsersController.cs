using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogCoreAPI.Authorization.Attributes;
using BlogCoreAPI.Authorization.Permissions;
using BlogCoreAPI.Models.Builders.Specifications;
using BlogCoreAPI.Models.Builders.Specifications.User;
using BlogCoreAPI.Models.DTOs.Comment;
using BlogCoreAPI.Models.DTOs.Like;
using BlogCoreAPI.Models.DTOs.Post;
using BlogCoreAPI.Models.DTOs.Role;
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
        /// Define default role(s) assigned to new users.
        /// </summary>
        /// <remarks>
        /// Define default role(s) assigned to new users.
        /// </remarks>
        /// <param name="defaultRoles"></param>
        /// <returns></returns>
        [HttpPost("Roles/Default")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DefineDefaultRolesToNewUsers(DefaultRolesDto defaultRoles)
        {
            // todo : possible improvement : do a AuthorizeAsync without resource parameter needed (only check if have right on all resource type specified)
            // Or something like that, so no more nedeed to give a not useful item (here user)
            var userId = int.Parse(User.Claims
                .First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);

            var user = await _userService.GetUserEntity(userId);
            // end
            
            var roleAuthorized = await _authorizationService.AuthorizeAsync(User, user, new PermissionRequirement(PermissionAction.CanUpdate, PermissionTarget.Role));
            if (!roleAuthorized.Succeeded)
                return Forbid();
            
            var userAuthorized = await _authorizationService.AuthorizeAsync(User, user, new PermissionRequirement(PermissionAction.CanUpdate, PermissionTarget.Account));
            if (!userAuthorized.Succeeded)
                return Forbid();
            
            await _userService.SetDefaultRolesAssignedToNewUsers(defaultRoles.Roles);
            return Ok();
        }
        
        /// <summary>
        /// Get current default role(s) assigned to new users.
        /// </summary>
        /// <remarks>
        /// Get current default role(s) assigned to new users.
        /// </remarks>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpGet("Roles/Default")]
        [PermissionWithPermissionRangeAllRequired(PermissionAction.CanRead, PermissionTarget.Role)]
        [ProducesResponseType(typeof(IEnumerable<GetRoleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetDefaultRolesToNewUsers()
        {
            return Ok(await _userService.GetDefaultRolesAssignedToNewUsers());
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
