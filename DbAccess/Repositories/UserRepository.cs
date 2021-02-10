using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbAccess.Data.POCO;
using DbAccess.DataContext;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(MyBlogContext context) : base(context)
        {
        }

        public IEnumerable<User> GetUsersById(IEnumerable<int> ids)
        {
            return Context.Set<User>().Where(x => ids.Contains(x.Id)).ToList();
        }
    }
}
