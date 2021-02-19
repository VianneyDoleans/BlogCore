using System;
using System.Linq;
using DbAccess.Data.POCO;
using Xunit;

namespace DBAccess.Test.Repositories
{
    public class PostsRepository : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public PostsRepository(DatabaseFixture databaseFixture)
        {
            _fixture = databaseFixture;
        }

        [Fact]
        public async void AddPostsAsync()
        {
            var repository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var testPosts = new Post() { Name = "This is a test for post" };
            await repository.AddAsync(testPosts);
            _fixture.UnitOfWork.Save();

            Assert.True(_fixture.Db.Posts.First(x => x.Name == testPosts.Name) != null);
        }

        [Fact]
        public async void AddNullPostsAsync()
        {
            var repository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.AddAsync(null));
        }

        [Fact]
        public async void GetPostsAsync()
        {
            var repository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var result = await repository.GetAsync(1);

            Assert.True(result == await _fixture.Db.Posts.FindAsync(1));
        }

        [Fact]
        public async void GetPostsOutOfRangeAsync()
        {
            var repository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);

            await Assert.ThrowsAsync<InvalidOperationException>(async () => await repository.GetAsync(100));
        }

        [Fact]
        public async void GetAllAsync()
        {
            var repository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var result = await repository.GetAllAsync();

            Assert.True(result.Count() == _fixture.Db.Posts.Count());
        }

        [Fact]
        public async void RemoveAsync()
        {
            var nbPostsAtBeginning = _fixture.Db.Posts.Count();
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var testPosts = new Post() { Name = "This is a test post" };

            await postRepository.AddAsync(testPosts);
            _fixture.UnitOfWork.Save();
            var nbPostsAfterAdded = _fixture.Db.Posts.Count();
            await postRepository.RemoveAsync(testPosts);
            _fixture.UnitOfWork.Save();
            var nbPostsAfterRemoved = _fixture.Db.Posts.Count();

            Assert.True(nbPostsAtBeginning + 1 == nbPostsAfterAdded &&
                        nbPostsAfterRemoved == nbPostsAtBeginning);
        }

        [Fact]
        public async void RemoveNullAsync()
        {
            var repository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.RemoveAsync(null));
        }
    }
}
