using System;
using System.Collections.Generic;
using System.Text;
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
                o.UseSqlServer(configuration.GetConnectionString("Default")));

            return services;
        }
    }
}
