using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DbAccess.Data.POCO.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using MyBlogAPI.Attributes;
using MyBlogAPI.Services.UserService;

namespace MyBlogAPI.Permissions
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        //private readonly UserService _userService;
        public PermissionAuthorizationHandler(/*UserService userService*/)
        {
            //_userService = userService;
        }

        public override async Task HandleAsync(AuthorizationHandlerContext context)
        {
            foreach (var req in context.Requirements.OfType<PermissionRequirement>())
            {
                await HandleRequirementAsync(context, req);
            }
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var routeValues = (context.Resource as HttpContext).Request.RouteValues;
            var controllerName = routeValues["controller"].ToString();
            var actionName = routeValues["action"].ToString();

            if (context.Resource is HttpContext httpContext)
            {
                var value = httpContext.GetRouteValue("key");
            }

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
