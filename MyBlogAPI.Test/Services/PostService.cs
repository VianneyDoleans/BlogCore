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

        [Fact]
        public async void AddNullPost()
        {

            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.AddPost(null));
        }

        [Fact]
        public async void UpdateNullPost()
        {

            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.UpdatePost(null));
        }

        [Fact]
        public async void AddPostWithTwoTimesSameTag()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "AddPstWiTwTiSaT@email.com", Password = "1234", Username = "AddPstWiTwTiSaT" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "AddPstWiTwTiSaT" });
            var tag1 = await _fixture.Db.Tags.AddAsync(
                new Tag() {Name = "AddPstWiTwTiSaT"});
            _fixture.UnitOfWork.Save();
            var post = new AddPostDto()
            {
                Author = user.Entity.Id, Category = category.Entity.Id, Content = "AddPstWiTwTiSaT", Name = "AddPstWiTwTiSaT",
                Tags = new List<int>(){ tag1.Entity.Id, tag1.Entity.Id }
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.AddPost(post));
        }

        [Fact]
        public async void AddPostWithInvalidTags()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "AddPstWithInvTags@email.com", Password = "1234", Username = "AddPstWithInvTags" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "AddPstWithInvTags" });
            var tag1 = await _fixture.Db.Tags.AddAsync(
                new Tag() { Name = "AddPstWithInvTags" });
            _fixture.UnitOfWork.Save();
            var post = new AddPostDto()
            {
                Author = user.Entity.Id, Category = category.Entity.Id, Content = "new post", Name = "AddPstWithInvTags",
                Tags = new List<int>() { tag1.Entity.Id, 126547 }
            };

            // Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.AddPost(post));
        }

        [Fact]
        public async void AddPostWithNullContent()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "AddPostWithNullContent@email.com", Password = "1234", Username = "AddPstWiNullCnt" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "AddPstWiNullCnt" });
            var tag1 = await _fixture.Db.Tags.AddAsync(
                new Tag() { Name = "AddPstWiNullCnt" });
            _fixture.UnitOfWork.Save();
            var post = new AddPostDto()
            {
                Author = user.Entity.Id,
                Category = category.Entity.Id,
                Name = "AddPstWiNullCnt",
                Tags = new List<int>() { tag1.Entity.Id }
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.AddPost(post));
        }

        [Fact]
        public async void AddPostWithNullName()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User()
                {
                    EmailAddress = "AddPostWithNullName@email.com", Password = "1234", Username = "AddPstWiNullNa"
                });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() {Name = "AddPstWiNullNa"});
            var tag1 = await _fixture.Db.Tags.AddAsync(
                new Tag() {Name = "AddPstWiNullNa"});
            _fixture.UnitOfWork.Save();
            var post = new AddPostDto()
            {
                Author = user.Entity.Id,
                Category = category.Entity.Id,
                Content = "AddPstWiNullNa",
                Tags = new List<int>() {tag1.Entity.Id}
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.AddPost(post));
        }

        [Fact]
        public async void AddPostWithNullAuthor()
        {
            // Arrange
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "AddPstWiNullAu" });
            var tag1 = await _fixture.Db.Tags.AddAsync(
                new Tag() { Name = "AddPstWiNullAu" });
            _fixture.UnitOfWork.Save();
            var post = new AddPostDto()
            {
                Name = "AddPstWiNullAu",
                Category = category.Entity.Id,
                Content = "AddPstWiNullAu",
                Tags = new List<int>() { tag1.Entity.Id }
            };

            // Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.AddPost(post));
        }

        [Fact]
        public async void AddPostWithNullCategory()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User()
                {
                    EmailAddress = "AddPostWithNullCa@email.com",
                    Password = "1234",
                    Username = "AddPstWiNullCa"
                });
            var tag1 = await _fixture.Db.Tags.AddAsync(
                new Tag() { Name = "AddPstWiNullCa" });
            _fixture.UnitOfWork.Save();
            var post = new AddPostDto()
            {
                Name = "AddPstWiNullCa",
                Author = user.Entity.Id,
                Content = "AddPstWiNullCa",
                Tags = new List<int>() { tag1.Entity.Id }
            };

            // Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.AddPost(post));
        }

        [Fact]
        public async void UpdatePostInvalid()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() {EmailAddress = "UpdatePostInvalid@email.com", Password = "1234", Username = "UpdatePostInvalid" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() {Name = "UpdatePostInv"});
            var tag1 = await _fixture.Db.Tags.AddAsync(
                new Tag() {Name = "UpdatePostInv"});
            _fixture.UnitOfWork.Save();
            var post = new AddPostDto()
                {Author = user.Entity.Id, Category = category.Entity.Id, Content = "new post", Name = "UpdatePostInvalid" };
            var postAdded = await _service.AddPost(post);
            var postToUpdate = new UpdatePostDto()
            {
                Id = postAdded.Id,
                Author = user.Entity.Id,
                Category = 154874,
                Name = "UpdatePostInvalid",
                Content = "UpdatePostInvalid",
                Tags = new List<int>() {tag1.Entity.Id}
            };

            // Act && Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.UpdatePost(postToUpdate));
        }

        [Fact]
        public async void GetPost()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "GetPost@email.com", Password = "1234", Username = "GetPost" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "GetPost" });
            _fixture.UnitOfWork.Save();
            var post = new AddPostDto() { Author = user.Entity.Id, Category = category.Entity.Id, Content = "new post", Name = "GetPost" };
            var postAdded = await _service.AddPost(post);

            var postDb = await _service.GetPost(postAdded.Id);

            // Act & Assert
            Assert.True(postDb.Id == postAdded.Id && 
                        postDb.Author == post.Author && 
                        postDb.Category == post.Category && 
                        postDb.Content == post.Content && 
                        postDb.Name == post.Name);
        }

        [Fact]
        public async void UpdatePost()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "UpdatePost@email.com", Password = "1234", Username = "UpdatePost" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "UpdatePost" });
            var tag1 = await _fixture.Db.Tags.AddAsync(
                new Tag() {Name = "UpdatePost" });
            _fixture.UnitOfWork.Save();
            var post = new AddPostDto() { Author = user.Entity.Id, Category = category.Entity.Id, Content = "new post", Name = "UpdatePost" };
            var postAdded = await _service.AddPost(post);
            var postToUpdate = new UpdatePostDto()
            {
                Id = postAdded.Id,
                Author = user.Entity.Id, Category = category.Entity.Id, Name = "UpdatePost", Content = "UpdatePost",
                Tags = new List<int>() {tag1.Entity.Id}
            };

            // Act
            await _service.UpdatePost(postToUpdate);

            // Assert
            var postDb = await _service.GetPost(postAdded.Id);
            Assert.True(postDb.Id == postAdded.Id &&
                        postDb.Author == post.Author &&
                        postDb.Category == post.Category &&
                        postDb.Content == post.Content &&
                        postDb.Name == post.Name &&
                        postDb.Tags.Count() == 1 &&
                        postDb.Tags.Contains(tag1.Entity.Id));
        }

        [Fact]
        public async void UpdatePostNotFound()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "UpdatePostNotFound@email.com", Password = "1234", Username = "UpdatePostNotFound" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "UpdatePostNotFound" });
            _fixture.UnitOfWork.Save();
            var postToUpdate = new UpdatePostDto()
            {
                Id = 124757,
                Author = user.Entity.Id,
                Category = category.Entity.Id,
                Name = "UpdatePostNotFound",
                Content = "UpdatePostNotFound"
            };

            // Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.UpdatePost(postToUpdate));
        }

        [Fact]
        public async void UpdatePostWithSameExistingProperties()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "UpPstWiSaExtProp@email.com", Password = "1234", Username = "UpPstWiSaExtProp" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "UpPstWiSaExtProp" });
            _fixture.UnitOfWork.Save();
            var post = new AddPostDto() { Author = user.Entity.Id, Category = category.Entity.Id, Content = "new post", Name = "UpPstWiSaExtProp" };
            var postAdded = await _service.AddPost(post);
            var postToUpdate = new UpdatePostDto()
            {
                Id = postAdded.Id,
                Author = user.Entity.Id,
                Category = category.Entity.Id,
                Content = "new post",
                Name = "UpPstWiSaExtProp"
            };

            // Act
            await _service.UpdatePost(postToUpdate);

            // Assert
            var postDb = await _service.GetPost(postAdded.Id);
            Assert.True(postDb.Id == postAdded.Id &&
                        postDb.Author == post.Author &&
                        postDb.Category == post.Category &&
                        postDb.Content == post.Content &&
                        postDb.Name == post.Name);
        }

        [Fact]
        public async void DeletePost()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "DeletePost@email.com", Password = "1234", Username = "DeletePost" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "DeletePost" });
            _fixture.UnitOfWork.Save();
            var post = new AddPostDto() { Author = user.Entity.Id, Category = category.Entity.Id, Content = "new post", Name = "DeletePost" };
            var postAdded = await _service.AddPost(post);

            // Act
            await _service.DeletePost(postAdded.Id);

            // Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.GetPost(postAdded.Id));
        }

        [Fact]
        public async void DeletePostNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.DeletePost(175574));
        }

        [Fact]
        public async void GetAllPosts()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "GetAllPosts@email.com", Password = "1234", Username = "GetAllPosts" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "GetAllPosts" });
            _fixture.UnitOfWork.Save();
            var post = new AddPostDto() { Author = user.Entity.Id, Category = category.Entity.Id, Content = "new post", Name = "GetAllPosts" };
            var post2 = new AddPostDto() { Author = user.Entity.Id, Category = category.Entity.Id, Content = "new post", Name = "GetAllPosts2" };
            var postAdded = await _service.AddPost(post);
            var postAdded2 = await _service.AddPost(post2);


            // Act
            var posts = (await _service.GetAllPosts()).ToArray();

            // Assert
            Assert.Contains(posts, x => x.Id == postAdded.Id && 
                                        x.Author == post.Author &&
                                        x.Category == post.Category &&
                                        x.Content == post.Content &&
                                        x.Name == post.Name);
            Assert.Contains(posts, x => x.Id == postAdded2.Id && 
                                        x.Author == post2.Author &&
                                        x.Category == post2.Category &&
                                        x.Content == post2.Content &&
                                        x.Name == post2.Name);
        }
    }
}
