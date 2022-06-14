using System.Collections.Generic;
using System.Threading.Tasks;
using BlogCoreAPI.DTOs.Permission;
using BlogCoreAPI.DTOs.Role;
using DBAccess.Data;
using DBAccess.Data.Permission;
using DBAccess.Specifications;
using DBAccess.Specifications.FilterSpecifications;
using DBAccess.Specifications.SortSpecification;

namespace BlogCoreAPI.Services.RoleService
{
    public interface IRoleService
    {
        Task<IEnumerable<GetRoleDto>> GetAllRoles();

        public Task<IEnumerable<GetRoleDto>> GetRoles(FilterSpecification<Role> filterSpecification = null,
            PagingSpecification pagingSpecification = null,
            SortSpecification<Role> sortSpecification = null);

        public Task<int> CountRolesWhere(FilterSpecification<Role> filterSpecification = null);

        Task<GetRoleDto> GetRole(int id);

        Task<Role> GetRoleEntity(int id);

        Task<GetRoleDto> AddRole(AddRoleDto role);

        Task UpdateRole(UpdateRoleDto role);

        Task DeleteRole(int id);

        Task AddPermissionAsync(int roleId, Permission permission);

        Task RemovePermissionAsync(int roleId, Permission permission);

        Task<IEnumerable<PermissionDto>> GetPermissionsAsync(int roleId);
    }
}
