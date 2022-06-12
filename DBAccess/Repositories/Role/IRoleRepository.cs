using System.Collections.Generic;
using System.Threading.Tasks;
using DBAccess.Data.POCO.Permission;

namespace DBAccess.Repositories.Role
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
        /// <param name="roleId"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        Task AddPermissionAsync(int roleId, Permission permission);

        /// <summary>
        /// Remove a permission from an existing role.
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        Task RemovePermissionAsync(int roleId, Permission permission);

        /// <summary>
        /// Get permissions from an existing role.
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<IEnumerable<Permission>> GetPermissionsAsync(int roleId);
    }
}
