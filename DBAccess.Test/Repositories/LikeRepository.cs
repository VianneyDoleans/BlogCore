using System;
using System.Linq;
using DbAccess.Data.POCO;
using Xunit;

namespace DBAccess.Test.Repositories
{
    public class LikesRepository : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public LikesRepository(DatabaseFixture databaseFixture)
        {
            _fixture = databaseFixture;
        }

        [Fact]
        public async void AddLikesAsync()
        {
            var repository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);
            var testLikes = new Like()
            {
                User = await _fixture.Db.Users.FindAsync(1), LikeableType = LikeableType.Post,
                Post = await _fixture.Db.Posts.FindAsync(1)
            };
            await repository.AddAsync(testLikes);
            _fixture.UnitOfWork.Save();

            Assert.True(_fixture.Db.Likes.First(x => x.Post == testLikes.Post) != null);
        }

        [Fact]
        public async void AddNullLikesAsync()
        {
            var repository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.AddAsync(null));
        }

        [Fact]
        public async void GetLikesAsync()
        {
            var repository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);
            var result = await repository.GetAsync(1);

            Assert.True(result == await _fixture.Db.Likes.FindAsync(1));
        }

        [Fact]
        public async void GetLikesOutOfRangeAsync()
        {
            var repository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);

            await Assert.ThrowsAsync<InvalidOperationException>(async () => await repository.GetAsync(100));
        }

        [Fact]
        public async void GetAllAsync()
        {
            var repository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);
            var result = await repository.GetAllAsync();

            Assert.True(result.Count() == _fixture.Db.Likes.Count());
        }

        [Fact]
        public async void RemoveAsync()
        {
            var nbLikesAtBeginning = _fixture.Db.Likes.Count();
            var tagRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);
            var testLikes = new Like()
            {
                User = await _fixture.Db.Users.FindAsync(1),
                LikeableType = LikeableType.Post,
                Post = await _fixture.Db.Posts.FindAsync(1)
            };

            await tagRepository.AddAsync(testLikes);
            _fixture.UnitOfWork.Save();
            var nbLikesAfterAdded = _fixture.Db.Likes.Count();
            await tagRepository.RemoveAsync(testLikes);
            _fixture.UnitOfWork.Save();
            var nbLikesAfterRemoved = _fixture.Db.Likes.Count();

            Assert.True(nbLikesAtBeginning + 1 == nbLikesAfterAdded &&
                        nbLikesAfterRemoved == nbLikesAtBeginning);
        }

        [Fact]
        public async void RemoveNullAsync()
        {
            var repository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.RemoveAsync(null));
        }
    }
}
