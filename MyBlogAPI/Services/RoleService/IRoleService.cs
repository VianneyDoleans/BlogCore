using System.Collections.Generic;
using System.Threading.Tasks;
using DbAccess.Data.POCO;
using DbAccess.Specifications;
using DbAccess.Specifications.SortSpecification;
using MyBlogAPI.DTO.Role;

namespace MyBlogAPI.Services.RoleService
{
    public interface IRoleService
    {
        Task<IEnumerable<GetRoleDto>> GetAllRoles();

        public Task<IEnumerable<GetRoleDto>> GetRoles(FilterSpecification<Role> filterSpecification = null,
            PagingSpecification pagingSpecification = null,
            SortSpecification<Role> sortSpecification = null);

        Task<GetRoleDto> GetRole(int id);

        Task<GetRoleDto> AddRole(AddRoleDto role);

        Task UpdateRole(UpdateRoleDto role);

        Task DeleteRole(int id);
    }
}
