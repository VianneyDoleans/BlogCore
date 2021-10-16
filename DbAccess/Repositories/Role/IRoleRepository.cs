using System.Collections.Generic;
using System.Threading.Tasks;

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
    }
}
