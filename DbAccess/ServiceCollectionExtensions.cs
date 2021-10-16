using System;
using DbAccess.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DbAccess
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MyBlogContext>(o =>
            {
                var dbType = configuration["DatabaseType"];
                switch (dbType)
                {
                    case "SQLServer":
                        o.UseSqlServer(
                            configuration.GetConnectionString("Default"));
                        break;
                    case "PostgreSQL":
                        o.UseNpgsql(
                            configuration.GetConnectionString("Default"));
                        break;
                    default:
                        throw new Exception("Unsupported database provider : " + dbType);
                }
            });

            return services;
        }
    }
}
