using System;
using System.Threading.Tasks;
using DbAccess;
using DbAccess.Data;
using DbAccess.Data.POCO;
using DbAccess.DataContext;
using DbAccess.Repositories.Category;
using DbAccess.Repositories.Comment;
using DbAccess.Repositories.Like;
using DbAccess.Repositories.Post;
using DbAccess.Repositories.Role;
using DbAccess.Repositories.Tag;
using DbAccess.Repositories.UnitOfWork;
using DbAccess.Repositories.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MyBlogAPI.Tests
{
    public class DatabaseFixture : IAsyncLifetime
    {
        public MyBlogContext Db { get; }
        public IUnitOfWork UnitOfWork { get; }
        public AutoMapperTestProfile MapperProfile { get; }

        public UserManager<User> UserManager { get; }
        public RoleManager<Role> RoleManager { get; }

        public async Task InitializeAsync()
        {
            await DbInitializer.Seed(Db, RoleManager, UserManager);
        }

        public Task DisposeAsync()
        {
            Db.Dispose();
            UnitOfWork.Dispose();
            return Task.CompletedTask;
        }

        public DatabaseFixture()
        {
            var provider = TestBootstrapper.GetProvider();

            UserManager = provider.GetRequiredService<UserManager<User>>();
            RoleManager = provider.GetRequiredService<RoleManager<Role>>();
            Db = provider.GetRequiredService<MyBlogContext>();
            UnitOfWork = new UnitOfWork(Db);

            MapperProfile = new AutoMapperTestProfile(new LikeRepository(Db), new UserRepository(Db, UserManager), new CategoryRepository(Db),
                new CommentRepository(Db), new RoleRepository(Db, RoleManager), new PostRepository(Db), new TagRepository(Db));
        }
    }
}
