using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DbAccess.Data.POCO;
using DbAccess.Data.POCO.Permission;
using Microsoft.AspNetCore.Authorization;
using MyBlogAPI.Attributes;
using MyBlogAPI.DTO.Permission;
using MyBlogAPI.Services.RoleService;
using MyBlogAPI.Services.UserService;

namespace MyBlogAPI.Permissions
{
    /// <summary>
    /// Authorization Handler used to authorize a resource when the <see cref="User"/> have <see cref="PermissionRange.All"/> permission on this Type of resource.
    /// </summary>
    /// <example>
    /// user "ModeratorName" have permission "PermissionRange.All, PermissionAction.Delete, PermissionTarget.Comment"
    /// => This handler requirement will succeed if this user ask authorization to delete a comment.
    /// </example>
    /// <example>
    /// user "classicUser" doesn't have permission "PermissionRange.All, PermissionAction.Read, PermissionTarget.Role"
    /// => This handler requirement won't succeed if this user ask authorization to read (GET) a role.
    /// </example>
    public class AllPermissionRangeAuthorizationHandler<TEntity> : AuthorizationHandler<PermissionRequirement, TEntity>
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        /// <inheritdoc />
        public AllPermissionRangeAuthorizationHandler(IUserService userService, IRoleService roleService, IMapper mapper)
        {
            _userService = userService;
            _roleService = roleService;
            _mapper = mapper;
        }

        /// <inheritdoc />
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement, TEntity entity)
        {
            var userId = int.Parse(context.User.Claims
                .First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);

            var user = await _userService.GetUser(userId);
            if (user.Roles.Any())
            {
                var requirementAction = _mapper.Map<PermissionActionDto>(requirement.Permission);
                var requirementTarget = _mapper.Map<PermissionTargetDto>(requirement.PermissionTarget);

                foreach (var role in user.Roles)
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
