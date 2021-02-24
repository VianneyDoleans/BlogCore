using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbAccess.DataContext;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Repositories.Role
{
    public class RoleRepository : Repository<Data.POCO.Role>, IRoleRepository
    {
        public RoleRepository(MyBlogContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Data.POCO.Role>> GetRolesFromUser(int id)
        {
            return await Context.Set<Data.POCO.JoiningEntity.UserRole>()
                .Where(x => x.UserId == id).Select(x => x.Role).ToListAsync();
        }

        public override async Task<Data.POCO.Role> GetAsync(int id)
        {
            return await Context.Set<Data.POCO.Role>().Include(x => x.UserRoles).SingleAsync(x => x.Id == id);
        }

        public override Data.POCO.Role Get(int id)
        {
            return Context.Set<Data.POCO.Role>().Include(x => x.UserRoles).Single(x => x.Id == id);
        }

        public override IEnumerable<Data.POCO.Role> GetAll()
        {
            return Context.Set<Data.POCO.Role>().Include(x => x.UserRoles).ToList();
        }

        public async Task<bool> NameAlreadyExists(string name)
        {
            var role = await Context.Set<Data.POCO.Role>().Where(x => x.Name == name).FirstOrDefaultAsync();
            return role != null;
        }

        public override async Task<IEnumerable<Data.POCO.Role>> GetAllAsync()
        {
            return await Context.Set<Data.POCO.Role>().Include(x => x.UserRoles).ToListAsync();
        }
    }
}
