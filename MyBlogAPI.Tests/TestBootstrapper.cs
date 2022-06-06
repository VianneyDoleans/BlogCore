using System;
using DbAccess.Data.POCO;
using DbAccess.DataContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MyBlogAPI.Tests
{
    public class TestBootstrapper
    {
        /// <summary>
        /// Create a service provider with an in-memory database context for testing.
        /// Use the service provider to get services and/or Database accesses.
        /// </summary>
        /// <returns></returns>
        public static ServiceProvider GetProvider()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<MyBlogContext, MsSqlDbContext>(o =>
                o.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<MyBlogContext>();
            var provider = services.BuildServiceProvider();
            return provider;
        }
    }
}
