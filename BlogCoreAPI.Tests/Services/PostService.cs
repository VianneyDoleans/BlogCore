using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlogCoreAPI.DTOs.Post;
using BlogCoreAPI.Services.PostService;
using BlogCoreAPI.Validators.Post;
using DBAccess.Data;
using DBAccess.Exceptions;
using DBAccess.Repositories.Category;
using DBAccess.Repositories.Post;
using DBAccess.Repositories.Tag;
using DBAccess.Repositories.User;
using FluentValidation;
using Xunit;

namespace BlogCoreAPI.Tests.Services
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
            _service = new BlogCoreAPI.Services.PostService.PostService(new PostRepository(_fixture.Db),
                mapper, _fixture.UnitOfWork, new UserRepository(_fixture.Db, _fixture.UserManager), new CategoryRepository(_fixture.Db),
                new TagRepository(_fixture.Db), new PostDtoValidator());
        }

        [Fact]
        public async Task AddPost()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "AddPost@email.com", Password = "1234", UserName = "AddPost" });
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
        public async Task AddPostWithAlreadyExistingName()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "AddPostAlreadyName@email.com", Password = "1234", UserName = "AddPostAlName" });
            var user2 = await _fixture.Db.Users.AddAsync(
                new User() { Email = "AddPostAlreadyName@email.com", Password = "1234", UserName = "AddPostAlName2" });
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
        public async Task AddPostWithTooLongName()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "AddPostTooLongName@email.com", Password = "1234", UserName = "AddPostTooName" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "AddPostLoName" });
            _fixture.UnitOfWork.Save();
            var post = new AddPostDto() { Author = user.Entity.Id, Category = category.Entity.Id, Content = "new post", 
                Name = "Ths is a long long long long long long long long long long long long long long long long long " +
                       "long long long long long long long long long long long long long long long long long long long " +
                       "long long long long long long long long long long long name !!" };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.AddPost(post));
        }

        [Fact]
        public async Task GetPostNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _service.GetPost(685479));
        }

        [Fact]
        public async Task UpdatePostOnlyOneProperty()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "UpdatePostOnlyOneProperty@email.com", Password = "1234", UserName = "UptPstOnlyOProp" });
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
        public async Task AddNullPost()
        {

            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.AddPost(null));
        }

        [Fact]
        public async Task UpdateNullPost()
        {

            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.UpdatePost(null));
        }

        [Fact]
        public async Task AddPostWithTwoTimesSameTag()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "AddPstWiTwTiSaT@email.com", Password = "1234", UserName = "AddPstWiTwTiSaT" });
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
        public async Task AddPostWithInvalidTags()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "AddPstWithInvTags@email.com", Password = "1234", UserName = "AddPstWithInvTags" });
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
            await Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _service.AddPost(post));
        }

        [Fact]
        public async Task AddPostWithNullContent()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "AddPostWithNullContent@email.com", Password = "1234", UserName = "AddPstWiNullCnt" });
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
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.AddPost(post));
        }

        [Fact]
        public async Task AddPostWithNullName()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User()
                {
                    Email = "AddPostWithNullName@email.com", Password = "1234", UserName = "AddPstWiNullNa"
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
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.AddPost(post));
        }

        [Fact]
        public async Task AddPostWithNullAuthor()
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
            await Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _service.AddPost(post));
        }

        [Fact]
        public async Task AddPostWithNullCategory()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User()
                {
                    Email = "AddPostWithNullCa@email.com",
                    Password = "1234",
                    UserName = "AddPstWiNullCa"
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
            await Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _service.AddPost(post));
        }

        [Fact]
        public async Task UpdatePostInvalid()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() {Email = "UpdatePostInvalid@email.com", Password = "1234", UserName = "UpdatePostInvalid" });
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
            await Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _service.UpdatePost(postToUpdate));
        }

        [Fact]
        public async Task UpdatePostMissingName()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "UpdatePostMissingName@email.com", Password = "1234", UserName = "UpPostMisName" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "UpPostMisName" });
            var tag1 = await _fixture.Db.Tags.AddAsync(
                new Tag() { Name = "UpPostMisName" });
            _fixture.UnitOfWork.Save();
            var post = new AddPostDto()
                { Author = user.Entity.Id, Category = category.Entity.Id, Content = "new post", Name = "UpPostMisName" };
            var postAdded = await _service.AddPost(post);
            var postToUpdate = new UpdatePostDto()
            {
                Id = postAdded.Id,
                Author = user.Entity.Id,
                Category = category.Entity.Id,
                Content = "UpPostMisName",
                Tags = new List<int>() { tag1.Entity.Id }
            };

            // Act && Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.UpdatePost(postToUpdate));
        }

        [Fact]
        public async Task UpdatePostMissingContent()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "UpdatePostMissingContent@email.com", Password = "1234", UserName = "UpPostMisContent" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "UpPostMisContent" });
            var tag1 = await _fixture.Db.Tags.AddAsync(
                new Tag() { Name = "UpPostMisContent" });
            _fixture.UnitOfWork.Save();
            var post = new AddPostDto()
                { Author = user.Entity.Id, Category = category.Entity.Id, Content = "new post", Name = "UpPostMisContent" };
            var postAdded = await _service.AddPost(post);
            var postToUpdate = new UpdatePostDto()
            {
                Id = postAdded.Id,
                Author = user.Entity.Id,
                Category = category.Entity.Id,
                Name = "UpPostMisContent",
                Tags = new List<int>() { tag1.Entity.Id }
            };

            // Act && Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.UpdatePost(postToUpdate));
        }

        [Fact]
        public async Task GetPost()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "GetPost@email.com", Password = "1234", UserName = "GetPost" });
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
        public async Task UpdatePost()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "UpdatePost@email.com", Password = "1234", UserName = "UpdatePost" });
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
                        postDb.Author == postToUpdate.Author &&
                        postDb.Category == postToUpdate.Category &&
                        postDb.Content == postToUpdate.Content &&
                        postDb.Name == postToUpdate.Name &&
                        postDb.Tags.Count() == 1 &&
                        postDb.Tags.Contains(tag1.Entity.Id));
        }

        [Fact]
        public async Task UpdatePostNotFound()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "UpdatePostNotFound@email.com", Password = "1234", UserName = "UpdatePostNotFound" });
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
            await Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _service.UpdatePost(postToUpdate));
        }

        [Fact]
        public async Task UpdatePostWithSomeUniqueExistingPropertiesFromAnotherPost()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "UpPstWiSoUExtProp@email.com", Password = "1234", UserName = "UpPstWiSoUExtProp" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "UpPstWiSoUExtProp" });
            _fixture.UnitOfWork.Save();
            var post = new AddPostDto() { Author = user.Entity.Id, Category = category.Entity.Id, Content = "new post", Name = "UpPstWiSoUExtProp" };
            var post2 = new AddPostDto() { Author = user.Entity.Id, Category = category.Entity.Id, Content = "new post", Name = "UpPstWiSoUExtProp2" };
            await _service.AddPost(post);
            var postAdded2 = await _service.AddPost(post2);
            var postToUpdate = new UpdatePostDto()
            {
                Id = postAdded2.Id,
                Author = user.Entity.Id,
                Category = category.Entity.Id,
                Content = "new post",
                Name = "UpPstWiSoUExtProp"
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.UpdatePost(postToUpdate));
        }

        [Fact]
        public async Task UpdatePostWithSameExistingProperties()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "UpPstWiSaExtProp@email.com", Password = "1234", UserName = "UpPstWiSaExtProp" });
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
        public async Task DeletePost()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "DeletePost@email.com", Password = "1234", UserName = "DeletePost" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "DeletePost" });
            _fixture.UnitOfWork.Save();
            var post = new AddPostDto() { Author = user.Entity.Id, Category = category.Entity.Id, Content = "new post", Name = "DeletePost" };
            var postAdded = await _service.AddPost(post);

            // Act
            await _service.DeletePost(postAdded.Id);

            // Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _service.GetPost(postAdded.Id));
        }

        [Fact]
        public async Task DeletePostNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _service.DeletePost(175574));
        }

        [Fact]
        public async Task GetAllPosts()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "GetAllPosts@email.com", Password = "1234", UserName = "GetAllPosts" });
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

        [Fact]
        public async Task GetPostsFromCategory()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "GetPostsFromCategory@email.com", Password = "1234", UserName = "GetPostsFromCat" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "GetPostsFromCat" });
            _fixture.UnitOfWork.Save();
            var post = new AddPostDto() { Author = user.Entity.Id, Category = category.Entity.Id, Content = "new post", Name = "GetPostsFromCat" };
            var post2 = new AddPostDto() { Author = user.Entity.Id, Category = category.Entity.Id, Content = "new post", Name = "GetPostsFromCat2" };
            var postAdded = await _service.AddPost(post);
            var postAdded2 = await _service.AddPost(post2);


            // Act
            var posts = (await _service.GetPostsFromCategory(category.Entity.Id)).ToArray();

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

        [Fact]
        public async Task GetPostsFromTag()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "GetPostsFromTag@email.com", Password = "1234", UserName = "GetPostsFromTag" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "GetPostsFromTag" });
            var tag = await _fixture.Db.Tags.AddAsync(new Tag() {Name = "GetPostsFromTag"});
            _fixture.UnitOfWork.Save();
            var post = new AddPostDto() { Author = user.Entity.Id, Category = category.Entity.Id, Content = "new post", Name = "GetPostsFromTag", Tags = new List<int>(){tag.Entity.Id}};
            var post2 = new AddPostDto() { Author = user.Entity.Id, Category = category.Entity.Id, Content = "new post", Name = "GetPostsFromTag2", Tags = new List<int>(){tag.Entity.Id}};
            var postAdded = await _service.AddPost(post);
            var postAdded2 = await _service.AddPost(post2);


            // Act
            var posts = (await _service.GetPostsFromTag(tag.Entity.Id)).ToArray();

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

        [Fact]
        public async Task GetPostsFromUser()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "GetPostsFromUser@email.com", Password = "1234", UserName = "GetPostsFromUser" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "GetPostsFromUser" });
            var tag = await _fixture.Db.Tags.AddAsync(new Tag() { Name = "GetPostsFromUser" });
            _fixture.UnitOfWork.Save();
            var post = new AddPostDto() { Author = user.Entity.Id, Category = category.Entity.Id, Content = "new post", Name = "GetPostsFromUser", Tags = new List<int>() { tag.Entity.Id } };
            var post2 = new AddPostDto() { Author = user.Entity.Id, Category = category.Entity.Id, Content = "new post", Name = "GetPostsFromUser2", Tags = new List<int>() { tag.Entity.Id } };
            var postAdded = await _service.AddPost(post);
            var postAdded2 = await _service.AddPost(post2);


            // Act
            var posts = (await _service.GetPostsFromUser(user.Entity.Id)).ToArray();

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
