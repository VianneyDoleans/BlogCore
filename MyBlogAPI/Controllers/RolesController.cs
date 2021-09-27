using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DbAccess.Specifications;
using MyBlogAPI.DTO.Role;
using MyBlogAPI.Filters;
using MyBlogAPI.Filters.Role;
using MyBlogAPI.Services.RoleService;
using MyBlogAPI.Services.UserService;

namespace MyBlogAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;

        public RolesController(IRoleService roleService, IUserService userService)
        {
            _roleService = roleService;
            _userService = userService;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _roleService.GetAllRoles());
        }

        [HttpGet()]
        public async Task<IActionResult> GetRoles(string sortingDirection = "ASC", int offset = 1,
            int limit = 10)
        {
            var validFilter = new PaginationFilter(offset, limit);

            return Ok(await _roleService.GetRoles(null,
                new PagingSpecification((validFilter.Offset - 1) * validFilter.Limit, validFilter.Limit),
                new SortRoleFilter(sortingDirection).GetSorting()));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _roleService.GetRole(id));
        }

        [HttpGet("{id:int}/Users/")]
        public async Task<IActionResult> GetUsersFromRole(int id)
        {
            return Ok(await _userService.GetUsersFromRole(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddTag(AddRoleDto role)
        {
            return Ok(await _roleService.AddRole(role));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTag(UpdateRoleDto role)
        {
            if (await _roleService.GetRole(role.Id) == null)
                return NotFound();
            await _roleService.UpdateRole(role);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            if (await _roleService.GetRole(id) == null)
                return NotFound();
            await _roleService.DeleteRole(id);
            return Ok();
        }
    }
}
