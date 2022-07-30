using System;
using System.IO;
using System.Linq;
using BlogCoreAPI.Models.DTOs.User;
using DBAccess;
using DBAccess.Data;
using DBAccess.DataContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using User = DBAccess.Data.User;

namespace BlogCoreAPI.FunctionalTests
{
    public class TestWebApplicationFactory : WebApplicationFactory<Startup>
    {
        private readonly string _dbName = Guid.NewGuid().ToString();
        public UserLoginDto Admin { get; set; }
        public UserLoginDto User { get; set; }



        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfiguration config = builder.Build();
            return config;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(async services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<BlogCoreContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();
                services.AddDbContext<BlogCoreContext>(options =>
                {
                    options.UseInMemoryDatabase(_dbName);
                    options.UseInternalServiceProvider(serviceProvider);
                });
                
                // Add AutoMapper profile
                services.AddAutoMapper(typeof(AutoMapperProfile));

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                await using var context = scope.ServiceProvider.GetRequiredService<BlogCoreContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
                await context.Database.EnsureCreatedAsync();
                // Fill Db with data
                await DbInitializer.Seed(context, roleManager, userManager);

                var configuration = GetConfiguration();
                var user = configuration.GetSection("Users").GetSection("User").Get<Models.User>();
                User = new UserLoginDto() { UserName = user.Name, Password = user.Password };
                var admin = configuration.GetSection("Users").GetSection("Admin").Get<Models.User>();
                Admin = new UserLoginDto() { UserName = admin.Name, Password = admin.Password };
            });
        }
    }
}
