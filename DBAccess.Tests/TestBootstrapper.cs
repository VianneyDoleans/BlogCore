using System;
using DBAccess.Data.POCO;
using DBAccess.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DBAccess.Tests
{
    public static class TestBootstrapper
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
            services.AddDbContext<BlogCoreContext, MsSqlDbContext>(o =>
                o.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<BlogCoreContext>();
            var provider = services.BuildServiceProvider();
            return provider;
        }
    }
}
