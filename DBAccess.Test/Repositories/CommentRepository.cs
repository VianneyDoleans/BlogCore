using System;
using System.Linq;
using DbAccess.Data.POCO;
using Xunit;

namespace DBAccess.Test.Repositories
{
    public class CommentRepository
    {
        private readonly DatabaseFixture _fixture;

        public CommentRepository()
        {
            _fixture = new DatabaseFixture();
        }

        [Fact]
        public async void AddCommentAsync()
        {
            var commentRepository = new DbAccess.Repositories.Comment.CommentsRepository(_fixture.Db);
            var testComment = new Comment()
                {Author = await _fixture.Db.Users.FindAsync(1), Content = "testContent test AddComment"};
            await commentRepository.AddAsync(testComment);
            _fixture.UnitOfWork.Save();

            Assert.True(_fixture.Db.Comments.First(x => x.Content == "testContent test AddComment") != null);
        }

        [Fact]
        public async void AddNullCommentAsync()
        {
            var commentRepository = new DbAccess.Repositories.Comment.CommentsRepository(_fixture.Db);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await commentRepository.AddAsync(null));
        }

        [Fact]
        public async void GetCommentAsync()
        {
            var commentRepository = new DbAccess.Repositories.Comment.CommentsRepository(_fixture.Db);
            var result = await commentRepository.GetAsync(1);

            Assert.True(result == await _fixture.Db.Comments.FindAsync(1));
        }

        [Fact]
        public async void GetCommentOutOfRangeAsync()
        {
            var commentRepository = new DbAccess.Repositories.Comment.CommentsRepository(_fixture.Db);

            await Assert.ThrowsAsync<InvalidOperationException>(async () => await commentRepository.GetAsync(100));
        }

        [Fact]
        public async void GetAllAsync()
        {
            var commentRepository = new DbAccess.Repositories.Comment.CommentsRepository(_fixture.Db);
            var result = await commentRepository.GetAllAsync();

            Assert.True(result.Count() == _fixture.Db.Comments.Count());
        }

        [Fact]
        public async void RemoveAsync()
        {
            var nbCommentsAtBeginning = _fixture.Db.Comments.Count();
            var commentRepository = new DbAccess.Repositories.Comment.CommentsRepository(_fixture.Db);
            var testComment = new Comment()
                {Author = await _fixture.Db.Users.FindAsync(1), Content = "testContent test AddComment"};

            await commentRepository.AddAsync(testComment);
            _fixture.UnitOfWork.Save();
            var nbCommentAfterAdded = _fixture.Db.Comments.Count();
            await commentRepository.RemoveAsync(testComment);
            _fixture.UnitOfWork.Save();
            var nbCommentAfterRemoved = _fixture.Db.Comments.Count();

            Assert.True(nbCommentsAtBeginning + 1 == nbCommentAfterAdded &&
                        nbCommentAfterRemoved == nbCommentsAtBeginning);
        }

        [Fact]
        public async void RemoveNullAsync()
        {
            var commentRepository = new DbAccess.Repositories.Comment.CommentsRepository(_fixture.Db);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await commentRepository.RemoveAsync(null));
        }
    }
}
