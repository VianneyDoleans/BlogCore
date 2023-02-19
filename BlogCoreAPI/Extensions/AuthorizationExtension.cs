using BlogCoreAPI.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace BlogCoreAPI.Extensions
{
    public static class AuthorizationExtension
    {
        public static IServiceCollection RegisterAuthorization(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.RegisterAuthorizationHandlers();
            return services;
        }
    }
}
