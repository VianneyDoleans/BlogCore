using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBAccess.Repositories.User
{
    public interface IUserRepository : IRepository<Data.User>
    {
        /// <summary>
        /// Method used to get a list of users by giving a list of user Ids
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<IEnumerable<Data.User>> GetUsersById(IEnumerable<int> ids);

        /// <summary>
        /// Method used to get users which possess a specific role by giving the Id of this role.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<Data.User>> GetUsersFromRole(int id);

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
        Task<bool> CheckPasswordAsync(Data.User user, string password);

        /// <summary>
        /// Add a role to a user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        Task AddRoleToUser(Data.User user, Data.Role role);

        /// <summary>
        /// remove a role to a user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        Task RemoveRoleToUser(Data.User user, Data.Role role);

        Task SetDefaultRolesToNewUsers(IEnumerable<Data.Role> roles);
        
        Task<IEnumerable<Data.Role>> GetDefaultRolesToNewUsers();

        /// <summary>
        /// Verify that the email confirmation token corresponds to the one sent to a user's email.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<bool> ConfirmEmail(string token, Data.User user);

        /// <summary>
        /// Verify that the password reset Token corresponds to the one sent to a user's email.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="user"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        Task ResetPassword(string token, Data.User user, string newPassword);
        
        /// <summary>
        /// Generate a token needed to confirm a user's email.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<string> GenerateEmailConfirmationToken(Data.User user);
        
        /// <summary>
        /// Generate a token needed to reset a user's email.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<string> GeneratePasswordResetToken(Data.User user);
    }
}
