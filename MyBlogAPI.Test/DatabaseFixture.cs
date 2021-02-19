using System;
using DbAccess.Data;
using DbAccess.DataContext;
using DbAccess.Repositories.UnitOfWork;

namespace DBAccess.Test
{
    public class DatabaseFixture : IDisposable
    {
        public MyBlogContext Db { get; private set; }
        public IUnitOfWork UnitOfWork { get; private set; }

        public DatabaseFixture()
        {
            Db = new MyBlogContext(
                TestBootstrapper.GetInMemoryDbContextOptions("testDb"));
            DbInitializer.Seed(Db);
            UnitOfWork = new UnitOfWork(Db);
        }

        public void Dispose()
        {
            Db.Database.EnsureDeleted();
        }
    }
}
