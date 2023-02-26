using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlogCoreAPI.Authorization.Permissions;
using BlogCoreAPI.Models.DTOs.Permission;
using BlogCoreAPI.Services.RoleService;
using BlogCoreAPI.Services.UserService;
using DBAccess.Data;
using DBAccess.Data.Permission;
using Microsoft.AspNetCore.Authorization;

namespace BlogCoreAPI.Authorization.PermissionHandlers.Resources
{
    /// <summary>
    /// Authorization Handler used to authorize a resource when the <see cref="User"/> have <see cref="PermissionRange.All"/> permission on this Type of resource.
    /// </summary>
    /// <example>
    /// user "ModeratorName" has a role that has the permission "PermissionRange.All, PermissionAction.Delete, PermissionTarget.Comment"
    /// => This handler requirement will succeed if this user ask authorization to delete a comment.
    /// </example>
    /// <example>
    /// user "classicUser" doesn't have a role that has permission "PermissionRange.All, PermissionAction.Read, PermissionTarget.Role"
    /// => This handler requirement won't succeed if this user ask authorization to read (GET) a role.
    /// </example>
    public class HasAllPermissionRangeAuthorizationHandler<TEntity> : AuthorizationHandler<PermissionRequirement, TEntity>
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        /// <inheritdoc />
        public HasAllPermissionRangeAuthorizationHandler(IUserService userService, IRoleService roleService, IMapper mapper)
        {
            _userService = userService;
            _roleService = roleService;
            _mapper = mapper;
        }

        /// <inheritdoc />
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement, TEntity resource)
        {
            var userId = int.Parse(context.User.Claims
                .First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);

            var account = await _userService.GetAccount(userId);
            if (account.Roles.Any())
            {
                var requirementAction = _mapper.Map<PermissionActionDto>(requirement.Permission);
                var requirementTarget = _mapper.Map<PermissionTargetDto>(requirement.PermissionTarget);

                foreach (var role in account.Roles)
                {
                    var permissions = await _roleService.GetPermissionsAsync(role);

                    if (permissions != null && permissions.Any(permission =>
                            requirementAction.Id == permission.PermissionAction.Id &&
                            requirementTarget.Id == permission.PermissionTarget.Id &&
                            permission.PermissionRange.Id == (int)PermissionRange.All))
                    {
                        context.Succeed(requirement);
                        return;
                    }
                }
            }
        }
    }
}
