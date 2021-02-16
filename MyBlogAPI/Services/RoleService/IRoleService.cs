using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBlogAPI.DTO;
using MyBlogAPI.DTO.Role;

namespace MyBlogAPI.Services.RoleService
{
    public interface IRoleService
    {
        Task<IEnumerable<GetRoleDto>> GetAllRoles();

        Task<GetRoleDto> GetRole(int id);

        Task AddRole(AddRoleDto role);

        Task UpdateRole(AddRoleDto role);

        Task DeleteRole(int id);
    }
}
