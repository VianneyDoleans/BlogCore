using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MyBlogAPI.DTO.Role;
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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _roleService.GetAllRoles());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _roleService.GetRole(id));
        }

        [HttpGet("{id}/Users/")]
        public async Task<IActionResult> GetUsersFromRole(int id)
        {
            return Ok(await _userService.GetUsersFromRole(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddTag(AddRoleDto role)
        {
            await _roleService.AddRole(role);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTag(AddRoleDto role)
        {
            if (await _roleService.GetRole(role.Id) == null)
                return NotFound();
            await _roleService.UpdateRole(role);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            if (await _roleService.GetRole(id) == null)
                return NotFound();
            await _roleService.DeleteRole(id);
            return Ok();
        }
    }
}
