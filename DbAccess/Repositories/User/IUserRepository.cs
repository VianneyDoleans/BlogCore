using System.Collections.Generic;
using System.Threading.Tasks;

namespace DbAccess.Repositories.User
{
    public interface IUserRepository : IRepository<Data.POCO.User>
    {
        Task<IEnumerable<Data.POCO.User>> GetUsersById(IEnumerable<int> ids);

        Task<IEnumerable<Data.POCO.User>> GetUsersFromRole(int id);

        Task<bool> UsernameAlreadyExists(string username);

        Task<bool> EmailAddressAlreadyExists(string emailAddress);
    }
}
