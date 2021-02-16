using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DbAccess.Repositories.Role
{
    public interface IRoleRepository : IRepository<Data.POCO.Role>
    {
        Task<IEnumerable<Data.POCO.Role>> GetRolesFromUser(int id);
    }
}
