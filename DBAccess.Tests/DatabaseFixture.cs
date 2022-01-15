using System;
using DbAccess.Data;
using DbAccess.DataContext;
using DbAccess.Repositories.UnitOfWork;

namespace DBAccess.Tests
{
    public class DatabaseFixture : IDisposable
    {
        public MyBlogContext Db { get; private set; }
        public IUnitOfWork UnitOfWork { get; private set; }

        public DatabaseFixture()
        {
            Db = new MsSqlDbContext(
                TestBootstrapper.GetInMemoryDbContextOptions());
            DbInitializer.Seed(Db);
            UnitOfWork = new UnitOfWork(Db);
        }

        public void Dispose()
        {
            Db.Dispose();
            UnitOfWork.Dispose();
        }
    }
}
