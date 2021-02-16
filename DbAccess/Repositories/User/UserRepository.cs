using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbAccess.DataContext;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Repositories.User
{
    public class UserRepository : Repository<Data.POCO.User>, IUserRepository
    {
        public UserRepository(MyBlogContext context) : base(context)
        {
        }

        public override async Task<Data.POCO.User> GetAsync(int id)
        {
            return await Context.Set<Data.POCO.User>().Include(x => x.UserRoles).SingleAsync(x => x.Id == id);
        }

        public override Data.POCO.User Get(int id)
        {
            return Context.Set<Data.POCO.User>().Include(x => x.UserRoles).Single(x => x.Id == id);
        }

        public override IEnumerable<Data.POCO.User> GetAll()
        {
            return Context.Set<Data.POCO.User>().Include(x => x.UserRoles).ToList();
        }

        public override async Task<IEnumerable<Data.POCO.User>> GetAllAsync()
        {
            return await Context.Set<Data.POCO.User>().Include(x => x.UserRoles).ToListAsync();
        }

        public async Task<IEnumerable<Data.POCO.User>> GetUsersById(IEnumerable<int> ids)
        {
            return await Context.Set<Data.POCO.User>().Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public async Task<IEnumerable<Data.POCO.User>> GetUsersFromRole(int id)
        {
            var userRole = Context.Set<Data.POCO.JoiningEntity.UserRole>().Include(x => x.Role)
                .Include(x => x.User)
                .Where(x => x.RoleId == id).ToList();
                var users = userRole
                .Select(y => y.User)
                .ToList();
                return users;
        }
    }
}
