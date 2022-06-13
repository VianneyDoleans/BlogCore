using System.Threading.Tasks;
using DBAccess;
using DBAccess.Data;
using DBAccess.DataContext;
using DBAccess.Repositories.Category;
using DBAccess.Repositories.Comment;
using DBAccess.Repositories.Like;
using DBAccess.Repositories.Post;
using DBAccess.Repositories.Role;
using DBAccess.Repositories.Tag;
using DBAccess.Repositories.UnitOfWork;
using DBAccess.Repositories.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BlogCoreAPI.Tests
{
    public class DatabaseFixture : IAsyncLifetime
    {
        public BlogCoreContext Db { get; }
        public IUnitOfWork UnitOfWork { get; }
        public AutoMapperTestProfile MapperProfile { get; }

        public UserManager<User> UserManager { get; }
        public RoleManager<Role> RoleManager { get; }

        public async Task InitializeAsync()
        {
            await DBInitializer.Seed(Db, RoleManager, UserManager);
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
            Db = provider.GetRequiredService<BlogCoreContext>();
            UnitOfWork = new UnitOfWork(Db);

            MapperProfile = new AutoMapperTestProfile(new LikeRepository(Db), new UserRepository(Db, UserManager), new CategoryRepository(Db),
                new CommentRepository(Db), new RoleRepository(Db, RoleManager), new PostRepository(Db), new TagRepository(Db));
        }
    }
}
