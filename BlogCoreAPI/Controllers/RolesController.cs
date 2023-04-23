using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using BlogCoreAPI.Authorization.Attributes;
using BlogCoreAPI.Models.Builders.Specifications;
using BlogCoreAPI.Models.Builders.Specifications.Role;
using BlogCoreAPI.Models.DTOs.Account;
using BlogCoreAPI.Models.DTOs.Role;
using BlogCoreAPI.Models.Queries;
using BlogCoreAPI.Responses;
using BlogCoreAPI.Services.RoleService;
using BlogCoreAPI.Services.UserService;
using DBAccess.Data.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogCoreAPI.Controllers
{
    /// <summary>
    /// Controller used to expose Role resources of the API.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentsController"/> class.
        /// </summary>
        /// <param name="roleService"></param>
        /// <param name="userService"></param>
        public RolesController(IRoleService roleService, IUserService userService)
        {
            _roleService = roleService;
            _userService = userService;
        }

        /// <summary>
        /// Get list of roles.
        /// </summary>
        /// <remarks>
        /// Get list of roles. The endpoint uses pagination and sort. Filter(s) can be applied for research.
        /// </remarks>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PagedBlogResponse<GetRoleDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRoles([FromQuery] GetRoleQueryParameters parameters)
        {
            var pagingSpecificationBuilder = new PagingSpecificationBuilder(parameters.Page, parameters.PageSize);
            var data = await _roleService.GetRoles(null,
                pagingSpecificationBuilder.Build(), new RoleSortSpecificationBuilder(parameters.OrderBy).Build());

            return Ok(new PagedBlogResponse<GetRoleDto>(data, pagingSpecificationBuilder.Page, pagingSpecificationBuilder.Limit,
                await _roleService.CountRolesWhere()));
        }

        /// <summary>
        /// Get a role by giving its Id.
        /// </summary>
        /// <remarks>
        /// Get a role by giving its Id.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetRoleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _roleService.GetRole(id));
        }
        
        /// <summary>
        /// Add a role.
        /// </summary>
        /// <remarks>
        /// Add a role.
        /// </remarks>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionWithPermissionRangeAllRequired(PermissionAction.CanCreate, PermissionTarget.Role)]
        [ProducesResponseType(typeof(GetRoleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddRole(AddRoleDto role)
        {
            return Ok(await _roleService.AddRole(role));
        }

        /// <summary>
        /// Update a role.
        /// </summary>
        /// <remarks>
        /// Update a role.
        /// </remarks>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPut]
        [PermissionWithPermissionRangeAllRequired(PermissionAction.CanUpdate, PermissionTarget.Role)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateRole(UpdateRoleDto role)
        {
            if (await _roleService.GetRole(role.Id) == null)
                return NotFound();
            await _roleService.UpdateRole(role);
            return Ok();
        }

        /// <summary>
        /// Get permissions from an existing role.
        /// </summary>
        /// <remarks>
        /// Get permissions from an existing role.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("/Roles/{id:int}/Permissions")]
        [PermissionWithPermissionRangeAllRequired(PermissionAction.CanRead, PermissionTarget.Permission)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPermissions(int id)
        {
            if (await _roleService.GetRole(id) == null)
                return NotFound();
            return Ok(JsonSerializer.Serialize(await _roleService.GetPermissionsAsync(id)));
        }

        /// <summary>
        /// Add a permission to an existing role.
        /// </summary>
        /// <remarks>
        /// Add a permission to an existing role.
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        [HttpPost("/Roles/{id:int}/Permissions")]
        [PermissionWithPermissionRangeAllRequired(PermissionAction.CanCreate, PermissionTarget.Permission)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddPermission(int id, Permission permission)
        {
            if (await _roleService.GetRole(id) == null)
                return NotFound();
            await _roleService.AddPermissionAsync(id, permission);
            return Ok();
        }

        /// <summary>
        /// Remove a permission to an existing role.
        /// </summary>
        /// <remarks>
        /// Remove a permission to an existing role.
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        [HttpDelete("/Roles/{id:int}/Permissions")]
        [PermissionWithPermissionRangeAllRequired(PermissionAction.CanDelete, PermissionTarget.Permission)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> RemovePermission(int id, Permission permission)
        {
            if (await _roleService.GetRole(id) == null)
                return NotFound();
            await _roleService.RemovePermissionAsync(id, permission);
            return Ok();
        }

        /// <summary>
        /// Delete a role by giving its id.
        /// </summary>
        /// <remarks>
        /// Delete a role by giving its id.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        [PermissionWithPermissionRangeAllRequired(PermissionAction.CanDelete, PermissionTarget.Role)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRole(int id)
        {
            if (await _roleService.GetRole(id) == null)
                return NotFound();
            await _roleService.DeleteRole(id);
            return Ok();
        }

        /// <summary>
        /// Get list of users with a specific role by giving the role's id.
        /// </summary>
        /// <remarks>
        /// Get list of users with a specific role by giving the role's id.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}/Users/")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<GetAccountDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BlogErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUsersFromRole(int id)
        {
            return Ok(await _userService.GetUsersFromRole(id));
        }
    }
}
