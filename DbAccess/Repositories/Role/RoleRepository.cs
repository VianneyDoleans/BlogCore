using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbAccess.DataContext;
using DbAccess.Specifications;
using DbAccess.Specifications.FilterSpecifications;
using DbAccess.Specifications.SortSpecification;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Repositories.Role
{
    public class RoleRepository : Repository<Data.POCO.Role>, IRoleRepository
    {
        public RoleRepository(MyBlogContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Data.POCO.Role>> GetAsync(FilterSpecification<Data.POCO.Role> filterSpecification = null,
            PagingSpecification pagingSpecification = null,
            SortSpecification<Data.POCO.Role> sortSpecification = null)
        {
            var query = GenerateQuery(filterSpecification, pagingSpecification, sortSpecification);
            return await query.Include(x => x.UserRoles).ToListAsync();
        }

        public async Task<IEnumerable<Data.POCO.Role>> GetRolesFromUser(int id)
        {
            return await Context.Set<Data.POCO.JoiningEntity.UserRole>()
                .Where(x => x.UserId == id).Select(x => x.Role).ToListAsync();
        }

        public override async Task<Data.POCO.Role> GetAsync(int id)
        {
            try
            {
                return await Context.Set<Data.POCO.Role>().Include(x => x.UserRoles).SingleAsync(x => x.Id == id);
            }
            catch
            {
                throw new IndexOutOfRangeException("Role doesn't exist.");
            }
        }

        public override Data.POCO.Role Get(int id)
        {
            try
            {
                return Context.Set<Data.POCO.Role>().Include(x => x.UserRoles).Single(x => x.Id == id);
            }
            catch
            {
                throw new IndexOutOfRangeException("Role doesn't exist.");
            }
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
