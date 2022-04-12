﻿using System.Collections.Generic;
using System.Threading.Tasks;
using DbAccess.Data.POCO;
using DbAccess.Data.POCO.Permission;
using DbAccess.Specifications;
using DbAccess.Specifications.FilterSpecifications;
using DbAccess.Specifications.SortSpecification;
using MyBlogAPI.DTO.Permission;
using MyBlogAPI.DTO.Role;

namespace MyBlogAPI.Services.RoleService
{
    public interface IRoleService
    {
        Task<IEnumerable<GetRoleDto>> GetAllRoles();

        public Task<IEnumerable<GetRoleDto>> GetRoles(FilterSpecification<Role> filterSpecification = null,
            PagingSpecification pagingSpecification = null,
            SortSpecification<Role> sortSpecification = null);

        public Task<int> CountRolesWhere(FilterSpecification<Role> filterSpecification = null);

        Task<GetRoleDto> GetRole(int id);

        Task<GetRoleDto> AddRole(AddRoleDto role);

        Task UpdateRole(UpdateRoleDto role);

        Task DeleteRole(int id);

        Task AddPermissionAsync(int roleId, Permission permission);

        Task RemovePermissionAsync(int roleId, Permission permission);

        Task<IEnumerable<PermissionDto>> GetPermissionsAsync(int roleId);
    }
}
