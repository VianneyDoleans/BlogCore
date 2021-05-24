using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbAccess.Repositories.Role
{
    public interface IRoleRepository : IRepository<Data.POCO.Role>
    {
        Task<IQueryable<Data.POCO.Role>> GetRolesFromUser(int id);

        Task<bool> NameAlreadyExists(string name);
    }
}
