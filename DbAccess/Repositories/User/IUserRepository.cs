using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbAccess.Repositories.User
{
    public interface IUserRepository : IRepository<Data.POCO.User>
    {
        Task<IQueryable<Data.POCO.User>> GetUsersById(IEnumerable<int> ids);

        Task<IQueryable<Data.POCO.User>> GetUsersFromRole(int id);

        Task<bool> UsernameAlreadyExists(string username);

        Task<bool> EmailAddressAlreadyExists(string emailAddress);
    }
}
