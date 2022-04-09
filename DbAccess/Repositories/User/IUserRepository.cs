using System.Collections.Generic;
using System.Threading.Tasks;

namespace DbAccess.Repositories.User
{
    public interface IUserRepository : IRepository<Data.POCO.User>
    {
        /// <summary>
        /// Method used to get a list of users by giving a list of user Ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<Data.POCO.User>> GetUsersById(IEnumerable<int> ids);

        /// <summary>
        /// Method used to get users which possess a specific role by giving the Id of this role.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<Data.POCO.User>> GetUsersFromRole(int id);

        /// <summary>
        /// Method used to check if an username already exists inside database.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<bool> UserNameAlreadyExists(string username);

        /// <summary>
        /// Method used to check if an emailAddress already exists inside database.
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        Task<bool> EmailAlreadyExists(string emailAddress);
    }
}
