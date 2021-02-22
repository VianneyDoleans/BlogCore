using System;
using AutoMapper;
using DbAccess.Data.POCO;
using DbAccess.Repositories.Post;
using MyBlogAPI.DTO.Post;
using MyBlogAPI.Services.PostService;
using Xunit;

namespace MyBlogAPI.Test.Services
{
    public class PostService : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;
        private readonly IPostService _service;

        public PostService(DatabaseFixture databaseFixture)
        {
            _fixture = databaseFixture;
            var config = new MapperConfiguration(cfg => { cfg.AddProfile(new AutoMapperProfile()); });
            var mapper = config.CreateMapper();
            _service = new MyBlogAPI.Services.PostService.PostService(new PostRepository(_fixture.Db),
                mapper, _fixture.UnitOfWork);
        }

        [Fact]
        public async void AddPost()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "AddPost@email.com", Password = "1234", Username = "postOk" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "AddCommentName" });
            _fixture.UnitOfWork.Save();
            var post = new AddPostDto() { Author = user.Entity.Id, Category = category.Entity.Id, Content = "new post", Name = "AddPost65" };

            // Act
            var postAdded = _service.AddPost(post);

            // Assert
            Assert.Contains(await _service.GetAllPosts(), x => x.Id == postAdded.Id &&
                                                               x.Author == post.Author &&
                                                               x.Category == post.Category &&
                                                               x.Content == post.Content &&
                                                               x.Name == post.Name);
        }

        [Fact]
        public async void AddPostWithAlreadyExistingName()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "AddPostAlready@email.com", Password = "1234", Username = "postOk11" });
            var user2 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "AddPostAlreadyExist@email.com", Password = "1234", Username = "postOk543" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "AddCommentName" });
            _fixture.UnitOfWork.Save();
            var post1 = new AddPostDto() { Author = user.Entity.Id, Category = category.Entity.Id, Content = "new post", Name = "AddPost65" };
            await _service.AddPost(post1);

            var post2 = new AddPostDto() { Author = user2.Entity.Id, Category = category.Entity.Id, Content = "new post", Name = "AddPost65" };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _service.AddPost(post2).Result);
        }

        [Fact]
        public async void AddPostWithTooLongName()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "AddPostTooLongName@email.com", Password = "1234", Username = "postOk132" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "AddPostLongNa" });
            _fixture.UnitOfWork.Save();
            var post = new AddPostDto() { Author = user.Entity.Id, Category = category.Entity.Id, Content = "new post", 
                Name = "Ths is a long long long long long long long long long long long long long long long long long " +
                       "long long long long long long long long long long long long long long long long long long long " +
                       "long long long long long long long long long long long name !!" };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _service.AddPost(post).Result);
        }

        [Fact]
        public void GetPostNotFound()
        {
            // Arrange & Act & Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.GetPost(685479).Result);
        }
    }
}
