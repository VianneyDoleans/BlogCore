using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbAccess.Specifications;

namespace DbAccess.Repositories.Role
{
    public interface IRoleRepository : IRepository<Data.POCO.Role>
    {
        Task<IEnumerable<Data.POCO.Role>> GetRolesFromUser(int id, PagingSpecification pagingSpecification = null);

        Task<bool> NameAlreadyExists(string name);
    }
}
