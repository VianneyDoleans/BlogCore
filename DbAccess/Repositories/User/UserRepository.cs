using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbAccess.DataContext;
using DbAccess.Specifications;
using DbAccess.Specifications.FilterSpecifications;
using DbAccess.Specifications.SortSpecification;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Repositories.User
{
    public class UserRepository : Repository<Data.POCO.User>, IUserRepository
    {
        private readonly UserManager<Data.POCO.User> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        public UserRepository(MyBlogContext context, UserManager<Data.POCO.User> userManager) : base(context)
        {
            _userManager = userManager;
        }

        /// <inheritdoc />
        public override async Task<IEnumerable<Data.POCO.User>> GetAsync(FilterSpecification<Data.POCO.User> filterSpecification = null,
            PagingSpecification pagingSpecification = null,
            SortSpecification<Data.POCO.User> sortSpecification = null)
        {
            var query = GenerateQuery(filterSpecification, pagingSpecification, sortSpecification);
            return await query.Include(x => x.UserRoles).ToListAsync();
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
        public override async Task<Data.POCO.User> AddAsync(Data.POCO.User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            var result = await _userManager.CreateAsync(user, user.Password);
            if (!result.Succeeded)
                throw new Exception(string.Concat(result.Errors.Select(x => x.Code + " : " + x.Description)));
            return await _userManager.FindByNameAsync(user.UserName);
        }

        /// <inheritdoc />
        public override async Task RemoveAsync(Data.POCO.User user)
        {
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                throw new Exception(string.Concat(result.Errors.Select(x => x.Code + " : " + x.Description)));
        }

        /// <inheritdoc />
        public override async Task RemoveRangeAsync(IEnumerable<Data.POCO.User> users)
        {
            if (users == null)
                throw new ArgumentNullException(nameof(users));
            foreach (var user in users)
            {
                await RemoveAsync(user);
            }
        }

        /// <inheritdoc />
        public override IEnumerable<Data.POCO.User> GetAll()
        {
            return Context.Set<Data.POCO.User>().Include(x => x.UserRoles).ToList();
        }

        /// <inheritdoc />
        public override async Task<IEnumerable<Data.POCO.User>> GetAllAsync()
        {
            return await Context.Set<Data.POCO.User>().Include(x => x.UserRoles).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Data.POCO.User>> GetUsersById(IEnumerable<int> ids)
        {
            return await Context.Set<Data.POCO.User>().Where(x => ids.Contains(x.Id)).Include(x => x.UserRoles).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Data.POCO.User>> GetUsersFromRole(int id)
        {
            var userRole = Context.Set<Data.POCO.JoiningEntity.UserRole>().Include(x => x.Role)
                .Include(x => x.User)
                .Where(x => x.RoleId == id);
                var users = await userRole
                .Select(y => y.User).ToListAsync();
                return users;
        }

        /// <inheritdoc />
        public async Task<bool> UserNameAlreadyExists(string username)
        {
            var user = await Context.Set<Data.POCO.User>().Where(x => x.UserName == username).FirstOrDefaultAsync();
            return user != null;
        }

        /// <inheritdoc />
        public async Task<bool> EmailAlreadyExists(string emailAddress)
        {
            var user = await Context.Set<Data.POCO.User>().Where(x => x.Email == emailAddress).FirstOrDefaultAsync();
            return user != null;
        }
    }
}
