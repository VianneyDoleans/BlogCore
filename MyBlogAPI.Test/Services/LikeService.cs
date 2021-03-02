using System;
using AutoMapper;
using DbAccess.Data.POCO;
using DbAccess.Repositories.Comment;
using DbAccess.Repositories.Like;
using DbAccess.Repositories.Post;
using DbAccess.Repositories.User;
using MyBlogAPI.DTO.Like;
using MyBlogAPI.Services.LikeService;
using Xunit;

namespace MyBlogAPI.Test.Services
{
    public class LikeService : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;
        private readonly ILikeService _service;

        public LikeService(DatabaseFixture databaseFixture)
        {
            _fixture = databaseFixture;
            var config = new MapperConfiguration(cfg => { cfg.AddProfile(databaseFixture.MapperProfile); });
            var mapper = config.CreateMapper();
            _service = new MyBlogAPI.Services.LikeService.LikeService(new LikeRepository(_fixture.Db),
                mapper, _fixture.UnitOfWork, new CommentRepository(_fixture.Db), new PostRepository(_fixture.Db), 
                new UserRepository(_fixture.Db));
        }

        [Fact]
        public async void AddLike()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "AddLike@email.com", Password = "1234", Username = "AddLike" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "AddLike" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post", Name = "AddLike" });
            _fixture.UnitOfWork.Save();
            var like = new AddLikeDto()
                {LikeableType = LikeableType.Post, Post = post.Entity.Id, User = user.Entity.Id};

            // Act
            var likeAdded = await _service.AddLike(like);

            // Assert
            Assert.Contains(await _service.GetAllLikes(), x => x.Id == likeAdded.Id &&
                                                                  x.Post == like.Post &&
                                                                  x.Comment == like.Comment &&
                                                                  x.User == like.User &&
                                                                  x.LikeableType == like.LikeableType);
        }

        [Fact]
        public async void AddLikeTwoTimesOnSameElement()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "AddLikeTwoTimes@email.com", Password = "1234", Username = "AddLikeTwoT" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "AddLikeTwoT" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post", Name = "AddLikeTwoT" });
            _fixture.UnitOfWork.Save();
            var like = new AddLikeDto()
                { LikeableType = LikeableType.Post, Post = post.Entity.Id, User = user.Entity.Id };
            await _service.AddLike(like);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.AddLike(like));
        }

        [Fact]
        public async void AddLikeWithNullLikeableElement()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "AddLikeNullElement@email.com", Password = "1234", Username = "AddLikeNullE" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "AddLikeNullE" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post", Name = "AddLikeNullE" });
            var comment = await _fixture.Db.Comments.AddAsync(new Comment()
                {Author = user.Entity, Content = "test", PostParent = post.Entity});
            _fixture.UnitOfWork.Save();
            var like = new AddLikeDto()
                { LikeableType = LikeableType.Post, User = user.Entity.Id, Comment = comment.Entity.Id};

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.AddLike(like));
        }

        [Fact]
        public async void GetLikeNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.GetLike(685479));
        }
    }
}
