using System;
using DbAccess.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MyBlogAPI.Test
{
    public class TestBootstrapper
    {
        /// <summary>
        /// Create an instance of in memory database context for testing.
        /// Use the returned DbContextOptions to initialize DbContext.
        /// </summary>
        /// <returns></returns>
        public static DbContextOptions<MyBlogContext> GetInMemoryDbContextOptions()
        {
            // The key to keeping the databases unique and not shared is 
            // generating a unique db name for each.
            var dbName = Guid.NewGuid().ToString();

            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var options = new DbContextOptionsBuilder<MyBlogContext>()
                .UseInMemoryDatabase(databaseName: dbName/*, new InMemoryDatabaseRoot()*/)
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            return options;
        }
    }
}
