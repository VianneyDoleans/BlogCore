using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DbAccess.Data.POCO;
using DbAccess.Repositories.Category;
using DbAccess.Repositories.Post;
using DbAccess.Repositories.Tag;
using DbAccess.Repositories.User;
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
            var config = new MapperConfiguration(cfg => { cfg.AddProfile(databaseFixture.MapperProfile); });
            var mapper = config.CreateMapper();
            _service = new MyBlogAPI.Services.PostService.PostService(new PostRepository(_fixture.Db),
                mapper, _fixture.UnitOfWork, new UserRepository(_fixture.Db), new CategoryRepository(_fixture.Db),
                new TagRepository(_fixture.Db));
        }

        [Fact]
        public async void AddPost()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "AddPost@email.com", Password = "1234", Username = "AddPost" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "AddPost" });
            _fixture.UnitOfWork.Save();
            var post = new AddPostDto() { Author = user.Entity.Id, Category = category.Entity.Id, Content = "new post", Name = "AddPost" };

            // Act
            var postAdded = await _service.AddPost(post);

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
                new User() { EmailAddress = "AddPostAlreadyName@email.com", Password = "1234", Username = "AddPostAlName" });
            var user2 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "AddPostAlreadyName@email.com", Password = "1234", Username = "AddPostAlName2" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "AddPostAlName" });
            _fixture.UnitOfWork.Save();
            var post1 = new AddPostDto() { Author = user.Entity.Id, Category = category.Entity.Id, Content = "new post", Name = "AddPostAlName" };
            await _service.AddPost(post1);

            var post2 = new AddPostDto() { Author = user2.Entity.Id, Category = category.Entity.Id, Content = "new post", Name = "AddPostAlName" };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.AddPost(post2));
        }

        [Fact]
        public async void AddPostWithTooLongName()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "AddPostTooLongName@email.com", Password = "1234", Username = "AddPostTooName" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "AddPostLoName" });
            _fixture.UnitOfWork.Save();
            var post = new AddPostDto() { Author = user.Entity.Id, Category = category.Entity.Id, Content = "new post", 
                Name = "Ths is a long long long long long long long long long long long long long long long long long " +
                       "long long long long long long long long long long long long long long long long long long long " +
                       "long long long long long long long long long long long name !!" };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.AddPost(post));
        }

        [Fact]
        public async void GetPostNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.GetPost(685479));
        }

        [Fact]
        public async void UpdatePostOnlyOneProperty()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "UpdatePostOnlyOneProperty@email.com", Password = "1234", Username = "UptPstOnlyOProp" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "UptPstOnlyOProp" });
            var tag1 = await _fixture.Db.Tags.AddAsync(
                new Tag() { Name = "UptPstOnlyOProp" });
            var tag2 = await _fixture.Db.Tags.AddAsync(
                new Tag() { Name = "UptPstOnlyOProp2" });
            _fixture.UnitOfWork.Save();
            var post = await _service.AddPost(new AddPostDto()
            {
                Author = user.Entity.Id,
                Category = category.Entity.Id,
                Content = "UpdatePostOnlyOneProperty",
                Name = "UpdatePostOnlyOneProperty",
                Tags = new List<int>() { tag1.Entity.Id, tag2.Entity.Id }
            });
            var postToUpdate = new UpdatePostDto()
            {
                Id = post.Id,
                Author = user.Entity.Id,
                Category = category.Entity.Id,
                Content = "UpdatePostOnlyOneProperty",
                Name = "UpdatePostOnlyOneProperty here",
                Tags = new List<int>() { tag2.Entity.Id, tag1.Entity.Id }
            };

            // Act
            await _service.UpdatePost(postToUpdate);

            // Assert
            var postUpdated = await _service.GetPost(post.Id);
            Assert.True(postUpdated.Category == postToUpdate.Category &&
                        postUpdated.Author == postToUpdate.Author &&
                        postUpdated.Content == postToUpdate.Content &&
                        postUpdated.Name == postToUpdate.Name &&
                        postUpdated.Tags.Contains(tag1.Entity.Id) &&
                        postUpdated.Tags.Contains(tag2.Entity.Id));
        }
    }
}
