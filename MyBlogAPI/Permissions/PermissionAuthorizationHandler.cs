using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DbAccess.Data.POCO.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MyBlogAPI.Attributes;
using MyBlogAPI.Services.UserService;

namespace MyBlogAPI.Permissions
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequiredAttribute>
    {
        //private readonly UserService _userService;
        public PermissionAuthorizationHandler(UserService userService)
        {
            //_userService = userService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequiredAttribute requirement)
        {
            var userPermissions =  context.User.Claims.Where(x => x.Type == "Permission" && x.Issuer == "LOCAL AUTHORITY").Select(x => JsonSerializer.Deserialize<Permission>(x.Value)).ToList();

            //var permissions = context.User.Claims.Where(x => x.Type == "Permission" &&
            //                                                 x.Value == requirement.Permission &&
            //                                                 x.Issuer == "LOCAL AUTHORITY");

            if (userPermissions.Any(x =>
                    x.PermissionTarget == requirement.PermissionTarget &&
                    x.PermissionAction == requirement.Permission))
            {
                context.Succeed(requirement);
                return;
            }
        }
    }
}
