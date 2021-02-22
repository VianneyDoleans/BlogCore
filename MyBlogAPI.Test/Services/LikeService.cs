using System;
using AutoMapper;
using DbAccess.Data.POCO;
using DbAccess.Repositories.Like;
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
            var config = new MapperConfiguration(cfg => { cfg.AddProfile(new AutoMapperProfile()); });
            var mapper = config.CreateMapper();
            _service = new MyBlogAPI.Services.LikeService.LikeService(new LikeRepository(_fixture.Db),
                mapper, _fixture.UnitOfWork);
        }

        [Fact]
        public async void AddLike()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "AddLike@email.com", Password = "1234", Username = "like243" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "AddLikeName" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post", Name = "AddLikeName" });
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
                new User() { EmailAddress = "AddLikeTwoTimes@email.com", Password = "1234", Username = "like6501" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "AddLikeTwo" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post", Name = "AddLikeTwo" });
            _fixture.UnitOfWork.Save();
            var like = new AddLikeDto()
                { LikeableType = LikeableType.Post, Post = post.Entity.Id, User = user.Entity.Id };
            await _service.AddLike(like);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _service.AddLike(like).Result);
        }

        [Fact]
        public async void AddLikeWithNullLikeableElement()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "AddLikeNullElement@email.com", Password = "1234", Username = "like912" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "AddLikeNullE" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post", Name = "AddLikeNullE" });
            var comment = await _fixture.Db.Comments.AddAsync(new Comment()
                {Author = user.Entity, Content = "test", PostParent = post.Entity});
            _fixture.UnitOfWork.Save();
            var like = new AddLikeDto()
                { LikeableType = LikeableType.Post, User = user.Entity.Id, Comment = comment.Entity.Id};
            await _service.AddLike(like);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _service.AddLike(like).Result);
        }

        [Fact]
        public void GetLikeNotFound()
        {
            // Arrange & Act & Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.GetLike(685479).Result);
        }
    }
}
