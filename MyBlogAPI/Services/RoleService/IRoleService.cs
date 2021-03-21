using System.Collections.Generic;
using System.Threading.Tasks;
using MyBlogAPI.DTO.Role;

namespace MyBlogAPI.Services.RoleService
{
    public interface IRoleService
    {
        Task<IEnumerable<GetRoleDto>> GetAllRoles();

        Task<GetRoleDto> GetRole(int id);

        Task<GetRoleDto> AddRole(AddRoleDto role);

        Task UpdateRole(UpdateRoleDto role);

        Task DeleteRole(int id);
    }
}
