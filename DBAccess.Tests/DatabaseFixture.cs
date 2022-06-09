using System;
using DbAccess.Data.POCO;
using DbAccess.DataContext;
using DbAccess.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DBAccess.Tests
{
    public sealed class DatabaseFixture : IDisposable
    {
        public MyBlogContext Db { get; }
        public RoleManager<Role> RoleManager { get; }
        public UserManager<User> UserManager { get; }
        public IUnitOfWork UnitOfWork { get; }

        public DatabaseFixture()
        {
            var provider = TestBootstrapper.GetProvider();

            UserManager = provider.GetRequiredService<UserManager<User>>();
            RoleManager = provider.GetRequiredService<RoleManager<Role>>();
            Db = provider.GetRequiredService<MyBlogContext>();
            UnitOfWork = new UnitOfWork(Db);
        }

        public void Dispose()
        {
            Db.Dispose();
            UnitOfWork.Dispose();
        }
    }
}
