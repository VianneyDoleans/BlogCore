using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DbAccess.Data.POCO.Permission;
using Microsoft.AspNetCore.Authorization;
using MyBlogAPI.Attributes;

namespace MyBlogAPI.Permissions
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequiredAttribute>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequiredAttribute requirement)
        {
            var userPermissions =  context.User.Claims.Where(x => x.Type == "Permission").Select(x => JsonSerializer.Deserialize<Permission>(x.Value)).ToList();

            //var permissions = context.User.Claims.Where(x => x.Type == "Permission" &&
            //                                                 x.Value == requirement.Permission &&
            //                                                 x.Issuer == "LOCAL AUTHORITY");

            //userPermissions.Any(x => x.PermissionTarget == requirement.PermissionTarget && x.PermissionAction == requirement.Permission &&
            //                         context.Resource)

            //if (permissions.Any())
            //{
            //    context.Succeed(requirement);
            //    return;
            //}
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
