using BlogCoreAPI.Services.JwtService;
using DBAccess.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlogCoreAPI.Extensions
{
    public static class JwtExtension
    {
        public static IServiceCollection RegisterJwt(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
            services.AddScoped<IJwtService, JwtService>();
            return services;
        }
    }
}
