using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbAccess.DataContext;
using DbAccess.Specifications;
using DbAccess.Specifications.FilterSpecifications;
using DbAccess.Specifications.SortSpecification;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Repositories.User
{
    public class UserRepository : Repository<Data.POCO.User>, IUserRepository
    {
        public UserRepository(MyBlogContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Data.POCO.User>> GetAsync(FilterSpecification<Data.POCO.User> filterSpecification = null,
            PagingSpecification pagingSpecification = null,
            SortSpecification<Data.POCO.User> sortSpecification = null)
        {
            var query = GenerateQuery(filterSpecification, pagingSpecification, sortSpecification);
            return await query.Include(x => x.UserRoles).ToListAsync();
        }

        public override async Task<Data.POCO.User> GetAsync(int id)
        {
            try
            {
                return await Context.Set<Data.POCO.User>().Include(x => x.UserRoles).SingleAsync(x => x.Id == id);
            }
            catch
            {
                throw new IndexOutOfRangeException("User doesn't exist.");
            }
        }

        public override Data.POCO.User Get(int id)
        {
            try
            {
                return Context.Set<Data.POCO.User>().Include(x => x.UserRoles).Single(x => x.Id == id);
            }
            catch
            {
                throw new IndexOutOfRangeException("User doesn't exist.");
            }
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
                .Where(x => x.RoleId == id);
                var users = await userRole
                .Select(y => y.User).ToListAsync();
                return users;
        }

        public async Task<bool> UsernameAlreadyExists(string username)
        {
            var user = await Context.Set<Data.POCO.User>().Where(x => x.Username == username).FirstOrDefaultAsync();
            return user != null;
        }

        public async Task<bool> EmailAddressAlreadyExists(string emailAddress)
        {
            var user = await Context.Set<Data.POCO.User>().Where(x => x.EmailAddress == emailAddress).FirstOrDefaultAsync();
            return user != null;
        }
    }
}
