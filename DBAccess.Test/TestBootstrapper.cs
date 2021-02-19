using System;
using System.Collections.Generic;
using System.Text;
using DbAccess.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DBAccess.Test
{
    public class TestBootstrapper
    {
        /// <summary>
        /// Create an instance of in memory database context for testing.
        /// Use the returned DbContextOptions to initialize DbContext.
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public static DbContextOptions<MyBlogContext> GetInMemoryDbContextOptions(string dbName = "Test_DB")
        {
            var options = new DbContextOptionsBuilder<MyBlogContext>()
                .UseInMemoryDatabase(databaseName: dbName, new InMemoryDatabaseRoot())
                .Options;

            return options;
        }
    }
}
