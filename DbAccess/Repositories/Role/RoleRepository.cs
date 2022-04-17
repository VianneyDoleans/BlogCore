using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using DbAccess.Data.POCO.Permission;
using DbAccess.DataContext;
using DbAccess.Specifications;
using DbAccess.Specifications.FilterSpecifications;
using DbAccess.Specifications.SortSpecification;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Repositories.Role
{
    public class RoleRepository : Repository<Data.POCO.Role>, IRoleRepository
    {
        private readonly RoleManager<Data.POCO.Role> _roleManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleRepository"/> class.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="roleManager"></param>
        public RoleRepository(MyBlogContext context, RoleManager<Data.POCO.Role> roleManager) : base(context)
        {
            _roleManager = roleManager;
        }

        /// <inheritdoc />
        public override async Task<IEnumerable<Data.POCO.Role>> GetAsync(FilterSpecification<Data.POCO.Role> filterSpecification = null,
            PagingSpecification pagingSpecification = null,
            SortSpecification<Data.POCO.Role> sortSpecification = null)
        {
            var query = GenerateQuery(filterSpecification, pagingSpecification, sortSpecification);
            return await query.Include(x => x.UserRoles).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Data.POCO.Role>> GetRolesFromUser(int id)
        {
            return await Context.Set<Data.POCO.JoiningEntity.UserRole>()
                .Where(x => x.UserId == id).Select(x => x.Role).ToListAsync();
        }

        /// <inheritdoc />
        public override async Task<Data.POCO.Role> AddAsync(Data.POCO.Role role)
        {
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
                throw new Exception(string.Concat(result.Errors.Select(x => x.Code + " : " + x.Description)));
            return await _roleManager.FindByNameAsync(role.Name);
        }

        /// <inheritdoc />
        public override async Task RemoveAsync(Data.POCO.Role role)
        {
            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
                throw new Exception(string.Concat(result.Errors.Select(x => x.Code + " : " + x.Description)));
        }

        /// <inheritdoc />
        public override async Task RemoveRangeAsync(IEnumerable<Data.POCO.Role> roles)
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
                throw new InvalidOperationException("This permission already exists for this role.");
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
                    throw new Exception(string.Concat(result.Errors.Select(x => x.Code + " : " + x.Description)));
            }
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
        public override IEnumerable<Data.POCO.Role> GetAll()
        {
            return Context.Set<Data.POCO.Role>().Include(x => x.UserRoles).ToList();
        }

        /// <inheritdoc />
        public async Task<bool> NameAlreadyExists(string name)
        {
            var role = await Context.Set<Data.POCO.Role>().Where(x => x.Name == name).FirstOrDefaultAsync();
            return role != null;
        }

        /// <inheritdoc />
        public override async Task<IEnumerable<Data.POCO.Role>> GetAllAsync()
        {
            return await Context.Set<Data.POCO.Role>().Include(x => x.UserRoles).ToListAsync();
        }
    }
}
