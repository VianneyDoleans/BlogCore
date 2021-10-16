﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DbAccess.Specifications;
using MyBlogAPI.DTO.Role;
using MyBlogAPI.Filters;
using MyBlogAPI.Filters.Role;
using MyBlogAPI.Responses;
using MyBlogAPI.Services.RoleService;
using MyBlogAPI.Services.UserService;

namespace MyBlogAPI.Controllers
{
    /// <summary>
    /// Controller used to expose Role resources of the API.
    /// </summary>
    [ApiController]
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
        /// <param name="sortingDirection"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> GetRoles(string sortingDirection = "ASC", int page = 1,
            int size = 10)
        {
            var validPagination = new PaginationFilter(page, size);
            var data = await _roleService.GetRoles(null,
                new PagingSpecification((validPagination.Offset - 1) * validPagination.Limit, validPagination.Limit),
                new SortRoleFilter(sortingDirection).GetSorting());

            return Ok(new PagedBlogResponse<GetRoleDto>(data, validPagination.Offset, validPagination.Limit,
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
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _roleService.GetRole(id));
        }

        /// <summary>
        /// /// <summary>
        /// Add a role.
        /// </summary>
        /// <remarks>
        /// Add a role.
        /// </remarks>
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
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
        public async Task<IActionResult> UpdateRole(UpdateRoleDto role)
        {
            if (await _roleService.GetRole(role.Id) == null)
                return NotFound();
            await _roleService.UpdateRole(role);
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
        public async Task<IActionResult> GetUsersFromRole(int id)
        {
            return Ok(await _userService.GetUsersFromRole(id));
        }
    }
}
