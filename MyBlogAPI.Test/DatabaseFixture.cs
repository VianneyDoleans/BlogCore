using System;
using DbAccess.Data;
using DbAccess.DataContext;
using DbAccess.Repositories.Category;
using DbAccess.Repositories.Comment;
using DbAccess.Repositories.Like;
using DbAccess.Repositories.Post;
using DbAccess.Repositories.Role;
using DbAccess.Repositories.Tag;
using DbAccess.Repositories.UnitOfWork;
using DbAccess.Repositories.User;

namespace MyBlogAPI.Test
{
    public class DatabaseFixture : IDisposable
    {
        public MyBlogContext Db { get; private set; }
        public IUnitOfWork UnitOfWork { get; private set; }
        public AutoMapperProfile MapperProfile { get; private set; }

        public DatabaseFixture()
        {
            Db = new MyBlogContext(
                TestBootstrapper.GetInMemoryDbContextOptions());
            DbInitializer.Seed(Db);
            UnitOfWork = new UnitOfWork(Db);
            MapperProfile = new AutoMapperProfile(new LikeRepository(Db), new UserRepository(Db), new CategoryRepository(Db),
                new CommentRepository(Db), new RoleRepository(Db), new PostRepository(Db), new TagRepository(Db));
        }

        public void Dispose()
        {
            Db.Dispose();//Database.EnsureDeleted();
            UnitOfWork.Dispose();
        }

    }
}
