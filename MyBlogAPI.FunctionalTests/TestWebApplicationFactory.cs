using System;
using System.Linq;
using DbAccess.Data.POCO;
using DbAccess.DataContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MyBlogAPI.FunctionalTests
{
    public class TestWebApplicationFactory : WebApplicationFactory<Startup>
    {
        private readonly string _dbName = Guid.NewGuid().ToString();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(async services =>
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
                
                // Add AutoMapper profile
                services.AddAutoMapper(typeof(AutoMapperProfile));

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                using var context = scope.ServiceProvider.GetRequiredService<MyBlogContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
                context.Database.EnsureCreatedAsync();
                // Fill Db with data
                await DbAccess.Data.DbInitializer.Seed(context, roleManager, userManager);
            });
        }
    }
}
