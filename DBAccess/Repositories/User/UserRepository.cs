using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBAccess.Data.JoiningEntity;
using DBAccess.DataContext;
using DBAccess.Exceptions;
using DBAccess.Specifications;
using DBAccess.Specifications.FilterSpecifications;
using DBAccess.Specifications.SortSpecification;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DBAccess.Repositories.User
{
    public class UserRepository : Repository<Data.User>, IUserRepository
    {
        private readonly UserManager<Data.User> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        public UserRepository(BlogCoreContext context, UserManager<Data.User> userManager) : base(context)
        {
            _userManager = userManager;
        }

        /// <inheritdoc />
        public override async Task<IEnumerable<Data.User>> GetAsync(FilterSpecification<Data.User> filterSpecification = null,
            PagingSpecification pagingSpecification = null,
            SortSpecification<Data.User> sortSpecification = null)
        {
            var query = GenerateQuery(filterSpecification, pagingSpecification, sortSpecification);
            return await query.Include(x => x.UserRoles).ToListAsync();
        }

        /// <inheritdoc />
        public override async Task<Data.User> GetAsync(int id)
        {
            try
            {
                return await _context.Set<Data.User>().Include(x => x.UserRoles).SingleAsync(x => x.Id == id);
            }
            catch
            {
                throw new ResourceNotFoundException("User doesn't exist.");
            }
        }

        /// <inheritdoc />
        public override Data.User Get(int id)
        {
            try
            {
                return _context.Set<Data.User>().Include(x => x.UserRoles).Single(x => x.Id == id);
            }
            catch
            {
                throw new ResourceNotFoundException("User doesn't exist.");
            }
        }

        /// <inheritdoc />
        public override async Task<Data.User> AddAsync(Data.User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            var result = await _userManager.CreateAsync(user, user.Password);
            if (!result.Succeeded)
                throw new Exception(string.Concat(result.Errors.Select(x => x.Code + " : " + x.Description)));
            var getUserAddedByManagerWithoutUserRolePropertyNavigation = await _userManager.FindByNameAsync(user.UserName);
            return await GetAsync(getUserAddedByManagerWithoutUserRolePropertyNavigation.Id);
        }

        /// <inheritdoc />
        public override async Task RemoveAsync(Data.User user)
        {
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                throw new Exception(string.Concat(result.Errors.Select(x => x.Code + " : " + x.Description)));
        }

        /// <inheritdoc />
        public override async Task RemoveRangeAsync(IEnumerable<Data.User> users)
        {
            if (users == null)
                throw new ArgumentNullException(nameof(users));
            foreach (var user in users)
            {
                await RemoveAsync(user);
            }
        }

        /// <inheritdoc />
        public override IEnumerable<Data.User> GetAll()
        {
            return _context.Set<Data.User>().Include(x => x.UserRoles).ToList();
        }

        /// <inheritdoc />
        public override async Task<IEnumerable<Data.User>> GetAllAsync()
        {
            return await _context.Set<Data.User>().Include(x => x.UserRoles).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Data.User>> GetUsersById(IEnumerable<int> ids)
        {
            return await _context.Set<Data.User>().Where(x => ids.Contains(x.Id)).Include(x => x.UserRoles).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Data.User>> GetUsersFromRole(int id)
        {
            var userRoles = await _context.Set<UserRole>().Include(x => x.Role)
                .Include(x => x.User)
                .Where(x => x.RoleId == id).ToListAsync();
            return userRoles.Select(y => y.User);
        }

        /// <inheritdoc />
        public async Task<bool> UserNameAlreadyExists(string username)
        {
            var user = await _context.Set<Data.User>().Where(x => x.UserName == username).FirstOrDefaultAsync();
            return user != null;
        }

        /// <inheritdoc />
        public async Task<bool> EmailAlreadyExists(string emailAddress)
        {
            var user = await _context.Set<Data.User>().Where(x => x.Email == emailAddress).FirstOrDefaultAsync();
            return user != null;
        }

        /// <inheritdoc />
        public async Task<bool> CheckPasswordAsync(Data.User user, string password)
        {
            var userSigninResult = await _userManager.CheckPasswordAsync(user, password);
            return userSigninResult;
        }

        public async Task AddRoleToUser(Data.User user, Data.Role role)
        {
            var result = await _userManager.AddToRoleAsync(user, role.Name);
            if (!result.Succeeded)
                throw new Exception(string.Concat(result.Errors.Select(x => x.Code + " : " + x.Description)));
        }

        public async Task RemoveRoleToUser(Data.User user, Data.Role role)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
            if (!result.Succeeded)
                throw new Exception(string.Concat(result.Errors.Select(x => x.Code + " : " + x.Description)));
        }
    }
}
