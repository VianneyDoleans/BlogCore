using System;
using DBAccess.Data;
using DBAccess.DataContext;
using DBAccess.Repositories.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DBAccess.Tests
{
    public sealed class DatabaseFixture : IDisposable
    {
        public BlogCoreContext Db { get; }
        public RoleManager<Role> RoleManager { get; }
        public UserManager<User> UserManager { get; }
        public IUnitOfWork UnitOfWork { get; }

        public DatabaseFixture()
        {
            var provider = TestBootstrapper.GetProvider();

            UserManager = provider.GetRequiredService<UserManager<User>>();
            RoleManager = provider.GetRequiredService<RoleManager<Role>>();
            Db = provider.GetRequiredService<BlogCoreContext>();
            UnitOfWork = new UnitOfWork(Db);
        }

        public void Dispose()
        {
            Db.Dispose();
            UnitOfWork.Dispose();
        }
    }
}
