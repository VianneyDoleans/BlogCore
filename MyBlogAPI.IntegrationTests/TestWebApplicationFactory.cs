using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbAccess.DataContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MyBlogAPI.IntegrationTests
{
    public class TestWebApplicationFactory : WebApplicationFactory<Startup>
    {
        private readonly string _dbName = Guid.NewGuid().ToString();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<MyBlogContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();
                services.AddDbContext<MyBlogContext>(options =>
                {
                    options.UseInMemoryDatabase(_dbName);
                    options.UseInternalServiceProvider(serviceProvider);
                });
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    using (var context = scope.ServiceProvider.GetRequiredService<MyBlogContext>())
                    {
                        try
                        {
                            context.Database.EnsureCreated();
                            // Fill Db with data
                            DbAccess.Data.DbInitializer.Seed(context);

                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }
                }
            });
        }
    }
}
