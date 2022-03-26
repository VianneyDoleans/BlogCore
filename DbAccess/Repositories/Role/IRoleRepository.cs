using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DbAccess.Data.POCO.Permission;

namespace DbAccess.Repositories.Role
{
    public interface IRoleRepository : IRepository<Data.POCO.Role>
    {
        /// <summary>
        /// Method used to see the roles of a user giving its Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<Data.POCO.Role>> GetRolesFromUser(int id);

        /// <summary>
        /// Method used to check if a name already exists inside database for a role.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<bool> NameAlreadyExists(string name);

        /// <summary>
        /// Add a permission to an existing role.
        /// </summary>
        /// <param name="role"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        Task AddPermissionAsync(Data.POCO.Role role, Permission permission);

        /// <summary>
        /// Remove a permission from an existing role.
        /// </summary>
        /// <param name="role"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        Task RemovePermissionAsync(Data.POCO.Role role, Permission permission);

        /// <summary>
        /// Get permissions from an existing role.
        /// </summary>
        /// <param name="role"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        Task<IEnumerable<Permission>> GetPermissionsAsync(Data.POCO.Role role);
    }
}
