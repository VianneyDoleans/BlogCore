using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using DBAccess.Data.JoiningEntity;
using DBAccess.Data.Permission;
using DBAccess.DataContext;
using DBAccess.Exceptions;
using DBAccess.Specifications;
using DBAccess.Specifications.FilterSpecifications;
using DBAccess.Specifications.SortSpecification;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DBAccess.Repositories.Role
{
    public class RoleRepository : Repository<Data.Role>, IRoleRepository
    {
        private readonly RoleManager<Data.Role> _roleManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleRepository"/> class.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="roleManager"></param>
        public RoleRepository(BlogCoreContext context, RoleManager<Data.Role> roleManager) : base(context)
        {
            _roleManager = roleManager;
        }

        /// <inheritdoc />
        public override async Task<IEnumerable<Data.Role>> GetAsync(FilterSpecification<Data.Role> filterSpecification = null,
            PagingSpecification pagingSpecification = null,
            SortSpecification<Data.Role> sortSpecification = null)
        {
            var query = GenerateQuery(filterSpecification, pagingSpecification, sortSpecification);
            return await query.Include(x => x.UserRoles).ToListAsync();
        }

        /// <inheritdoc />
        public override async Task<Data.Role> GetAsync(int id)
        {
            try
            {
                return await _context.Set<Data.Role>().Include(x => x.UserRoles).SingleAsync(x => x.Id == id);
            }
            catch
            {
                throw new ResourceNotFoundException("Role doesn't exist.");
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Data.Role>> GetRolesFromUser(int id)
        {
            var userRoles = await _context.Set<UserRole>()
                .Include(x => x.Role)
                .Include(x => x.User)
                .Where(x => x.UserId == id).ToListAsync();
            return userRoles.Select(x => x.Role);
        }

        /// <inheritdoc />
        public override async Task<Data.Role> AddAsync(Data.Role role)
        {
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
                throw new RoleManagementException(string.Concat(result.Errors.Select(x => x.Code + " : " + x.Description)));
            return await _roleManager.FindByNameAsync(role.Name);
        }

        /// <inheritdoc />
        public override async Task RemoveAsync(Data.Role role)
        {
            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
                throw new RoleManagementException(string.Concat(result.Errors.Select(x => x.Code + " : " + x.Description)));
        }

        /// <inheritdoc />
        public override async Task RemoveRangeAsync(IEnumerable<Data.Role> roles)
        {
            if (roles == null)
               throw new ArgumentNullException(nameof(roles));
            foreach (var entity in roles)
            {
                await RemoveAsync(entity);
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Permission>> GetPermissionsAsync(int roleId)
        {
            var role = await GetAsync(roleId);
            var claims = await _roleManager.GetClaimsAsync(role);

            return claims.Select(claim => JsonSerializer.Deserialize<Permission>(claim.Value)).ToList();
        }

        /// <inheritdoc />
        public async Task AddPermissionAsync(int roleId, Permission permission)
        {
            if (permission == null)
                throw new ArgumentNullException(nameof(permission),"permission cannot be null.");
            var role = await GetAsync(roleId);
            var allClaims = await _roleManager.GetClaimsAsync(role);
            if (allClaims.Any(x => x.Type == "Permission" && permission.Equals(JsonSerializer.Deserialize<Permission>(x.Value)))) 
                throw new PermissionManagementException("This permission already exists for this role.");
            await _roleManager.AddClaimAsync(role, new Claim("Permission", JsonSerializer.Serialize(permission)));
        }

        /// <inheritdoc />
        public async Task RemovePermissionAsync(int roleId, Permission permission)
        {
            if (permission == null)
                throw new ArgumentNullException(nameof(permission), "permission cannot be null.");
            var role = await GetAsync(roleId);
            var allClaims = await _roleManager.GetClaimsAsync(role);
            var claim =  allClaims.FirstOrDefault(x => x.Type == "Permission" && permission.Equals(JsonSerializer.Deserialize<Permission>(x.Value)));
            if (claim != null)
            {
                var result = await _roleManager.RemoveClaimAsync(role, claim);
                if (!result.Succeeded)
                    throw new RoleManagementException(string.Concat(result.Errors.Select(x => x.Code + " : " + x.Description)));
            }
        }

        /// <inheritdoc />
        public override Data.Role Get(int id)
        {
            try
            {
                return _context.Set<Data.Role>().Include(x => x.UserRoles).Single(x => x.Id == id);
            }
            catch
            {
                throw new ResourceNotFoundException("Role doesn't exist.");
            }
        }

        /// <inheritdoc />
        public override IEnumerable<Data.Role> GetAll()
        {
            return _context.Set<Data.Role>().Include(x => x.UserRoles).ToList();
        }

        /// <inheritdoc />
        public async Task<bool> NameAlreadyExists(string name)
        {
            var role = await _context.Set<Data.Role>().Where(x => x.Name == name).FirstOrDefaultAsync();
            return role != null;
        }

        /// <inheritdoc />
        public override async Task<IEnumerable<Data.Role>> GetAllAsync()
        {
            return await _context.Set<Data.Role>().Include(x => x.UserRoles).ToListAsync();
        }
    }
}
