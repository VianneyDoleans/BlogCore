using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBAccess.Repositories.User
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

        /// <summary>
        /// Check if password given is valid for this user (sign up).
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> CheckPasswordAsync(Data.POCO.User user, string password);

        /// <summary>
        /// Add a role to a user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        Task AddRoleToUser(Data.POCO.User user, Data.POCO.Role role);

        /// <summary>
        /// remove a role to a user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        Task RemoveRoleToUser(Data.POCO.User user, Data.POCO.Role role);
    }
}
