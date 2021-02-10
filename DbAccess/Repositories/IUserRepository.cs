using System;
using System.Collections.Generic;
using System.Text;
using DbAccess.Data.POCO;

namespace DbAccess.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        IEnumerable<User> GetUsersById(IEnumerable<int> ids);
    }
}
