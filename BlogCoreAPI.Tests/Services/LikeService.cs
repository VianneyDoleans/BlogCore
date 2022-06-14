using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlogCoreAPI.DTOs.Like;
using BlogCoreAPI.Services.LikeService;
using BlogCoreAPI.Validators.Like;
using DBAccess.Data;
using DBAccess.Exceptions;
using DBAccess.Repositories.Comment;
using DBAccess.Repositories.Like;
using DBAccess.Repositories.Post;
using DBAccess.Repositories.User;
using FluentValidation;
using Xunit;

namespace BlogCoreAPI.Tests.Services
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
            _service = new BlogCoreAPI.Services.LikeService.LikeService(new LikeRepository(_fixture.Db),
                mapper, _fixture.UnitOfWork, new CommentRepository(_fixture.Db), new PostRepository(_fixture.Db), 
                new UserRepository(_fixture.Db, _fixture.UserManager), new LikeDtoValidator());
        }

        [Fact]
        public async Task AddLike()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "AddLike@email.com", Password = "1234", UserName = "AddLike" });
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
        public async Task AddLikeTwoTimesOnSameElement()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "AddLikeTwoTimes@email.com", Password = "1234", UserName = "AddLikeTwoT" });
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
        public async Task AddLikeWithNullLikeableElementPost()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "AddLikeNullElementPost@email.com", Password = "1234", UserName = "AddLikeNullEP" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "AddLikeNullEP" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post", Name = "AddLikeNullEP" });
            var comment = await _fixture.Db.Comments.AddAsync(new Comment()
                {Author = user.Entity, Content = "test", PostParent = post.Entity});
            _fixture.UnitOfWork.Save();
            var like = new AddLikeDto()
                { LikeableType = LikeableType.Post, User = user.Entity.Id, Comment = comment.Entity.Id};

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.AddLike(like));
        }

        [Fact]
        public async Task AddLikeWithNullLikeableElementComment()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "AddLikeNullElementC@email.com", Password = "1234", UserName = "AddLikeNullEC" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "AddLikeNullEC" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post", Name = "AddLikeNullEC" });
            _fixture.UnitOfWork.Save();
            var like = new AddLikeDto()
                { LikeableType = LikeableType.Comment, User = user.Entity.Id, Post = post.Entity.Id };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.AddLike(like));
        }

        [Fact]
        public async Task AddLikeWithTwoLikeableElement()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "AddLikeTwElement@email.com", Password = "1234", UserName = "AddLikeTwE" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "AddLikeTwE" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post", Name = "AddLikeTwE" });
            var comment = await _fixture.Db.Comments.AddAsync(new Comment()
                { Author = user.Entity, Content = "test", PostParent = post.Entity });
            _fixture.UnitOfWork.Save();
            var like = new AddLikeDto()
                { LikeableType = LikeableType.Post, User = user.Entity.Id, Comment = comment.Entity.Id, Post = post.Entity.Id };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.AddLike(like));
        }

        [Fact]
        public async Task AddLikeWithNullUser()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "AddLikeNullU@email.com", Password = "1234", UserName = "AddLikeNullU" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "AddLikeNullU" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post", Name = "AddLikeNullU" });
            _fixture.UnitOfWork.Save();
            var like = new AddLikeDto()
                { LikeableType = LikeableType.Post, Post = post.Entity.Id };

            // Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _service.AddLike(like));
        }

        [Fact]
        public async Task AddLikeWithOnlyLikeableType()
        {
            // Arrange
            var like = new AddLikeDto()
                { LikeableType = LikeableType.Post };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.AddLike(like));
        }

        [Fact]
        public async Task GetLike()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() {Email = "GetLike@email.com", Password = "1234", UserName = "GetLike" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() {Name = "GetLike" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post", Name = "GetLike" });
            _fixture.UnitOfWork.Save();
            var likeToAdd = new AddLikeDto() {LikeableType = LikeableType.Post, Post = post.Entity.Id, User = user.Entity.Id};

            // Act
            var likeDb = await _service.GetLike((await _service.AddLike(likeToAdd)).Id);

            // Assert
            Assert.True(likeDb.Post == likeToAdd.Post &&
                        likeDb.User == likeToAdd.User &&
                        likeDb.Comment == likeToAdd.Comment &&
                        likeDb.LikeableType == likeToAdd.LikeableType);
        }

        [Fact]
        public async Task GetLikeNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _service.GetLike(685479));
        }

        [Fact]
        public async Task UpdateLike()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "UpdateLike@email.com", Password = "1234", UserName = "UpdateLike" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "AddLikeNullU" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post", Name = "UpdateLike"});
            var comment = await _fixture.Db.Comments.AddAsync(
                new Comment() { Author = user.Entity, Content = "test", PostParent = post.Entity });
            _fixture.UnitOfWork.Save();
            var like = await _service.AddLike(new AddLikeDto()
                { LikeableType = LikeableType.Post, Post = post.Entity.Id, User = user.Entity.Id });
            var likeToUpdate = new UpdateLikeDto()
                {Id = like.Id, LikeableType = LikeableType.Comment, Comment = comment.Entity.Id, User = user.Entity.Id };

            // Act
            await _service.UpdateLike(likeToUpdate);


            // Assert
            var likeDb = await _service.GetLike(likeToUpdate.Id);
            Assert.True(likeDb.Post == likeToUpdate.Post &&
                        likeDb.User == likeToUpdate.User &&
                        likeDb.Comment == likeToUpdate.Comment &&
                        likeDb.LikeableType == likeToUpdate.LikeableType);
        }

        [Fact]
        public async Task UpdateLikeNotFound()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "UpdateLikeNotFound@email.com", Password = "1234", UserName = "UpdateLikeNotFound" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "UpdateLikeNotFound" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post", Name = "UpdateLikeNotFound" });
            _fixture.UnitOfWork.Save();

            // Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _service.UpdateLike(new UpdateLikeDto()
            { Id = 164854, LikeableType = LikeableType.Post, Post = post.Entity.Id, User = user.Entity.Id}));
        }

        [Fact]
        public async Task UpdateLikeWithSameExistingProperties()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "UpdateLikeWithSameExistingPropertie@email.com", Password = "1234", UserName = "UpLikeWithSmExtPro" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "UpLikeWithSmExtPro" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post", Name = "UpLikeWithSmExtPro" });
            var comment = await _fixture.Db.Comments.AddAsync(new Comment()
                { Author = user.Entity, Content = "test", PostParent = post.Entity });
            _fixture.UnitOfWork.Save();
            var like = await _service.AddLike(new AddLikeDto()
                { LikeableType = LikeableType.Comment, Comment = comment.Entity.Id, User = user.Entity.Id });
            var likeToUpdate = new UpdateLikeDto()
                { Id = like.Id, LikeableType = LikeableType.Comment, Comment = comment.Entity.Id, User = user.Entity.Id };

            // Act
            await _service.UpdateLike(likeToUpdate);

            // Assert
            var likeDb = await _service.GetLike(likeToUpdate.Id);
            Assert.True(likeDb.Post == like.Post &&
                        likeDb.User == like.User &&
                        likeDb.Comment == like.Comment &&
                        likeDb.LikeableType == like.LikeableType);
        }

        [Fact]
        public async Task UpdateLikeInvalid()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "UpdateLikeInvalid@email.com", Password = "1234", UserName = "UpdateLikeInvalid" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "UpdateLikeInvalid" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() {Author = user.Entity, Category = category.Entity, Content = "new post", Name = "UpdateLikeInvalid"});
            _fixture.UnitOfWork.Save();
            var like = await _service.AddLike(new AddLikeDto()
                { LikeableType = LikeableType.Post, Post = post.Entity.Id, User = user.Entity.Id });
            var likeToUpdate = new UpdateLikeDto()
                { Id = like.Id, LikeableType = LikeableType.Comment, Post = post.Entity.Id, User = user.Entity.Id };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.UpdateLike(likeToUpdate));
        }

        [Fact]
        public async Task DeleteLike()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "DeleteLike@email.com", Password = "1234", UserName = "DeleteLike" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "DeleteLike" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() {Author = user.Entity, Category = category.Entity, Content = "new post", Name = "DeleteLike"});
            _fixture.UnitOfWork.Save();
            var likeToAdd = new AddLikeDto()
                { LikeableType = LikeableType.Post, Post = post.Entity.Id, User = user.Entity.Id };
            var like = await _service.AddLike(likeToAdd);

            // Act
            await _service.DeleteLike(like.Id);

            // Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _service.GetLike(like.Id));
        }

        [Fact]
        public async Task DeleteLikeNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _service.DeleteLike(175574));
        }

        [Fact]
        public async Task GetAllLikes()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "GetAllLikes@email.com", Password = "1234", UserName = "GetAllLikes" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "GetAllLikes" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() {Author = user.Entity, Category = category.Entity, Content = "new post", Name = "GetAllLikes"});
            var comment = await _fixture.Db.Comments.AddAsync(new Comment()
                { Author = user.Entity, Content = "test", PostParent = post.Entity });
            _fixture.UnitOfWork.Save();
            var likeToAdd = new AddLikeDto()
                { LikeableType = LikeableType.Post, Post = post.Entity.Id, User = user.Entity.Id };
            var likeToAdd2 = new AddLikeDto()
                { LikeableType = LikeableType.Comment, Comment = comment.Entity.Id, User = user.Entity.Id };
            await _service.AddLike(likeToAdd);
            await _service.AddLike(likeToAdd2);

            // Act
            var likes = (await _service.GetAllLikes()).ToArray();

            // Assert
            Assert.Contains(likes, x => x.Post == likeToAdd.Post &&
                                     x.Comment == likeToAdd.Comment &&
                                     x.LikeableType == likeToAdd.LikeableType &&
                                     x.User == likeToAdd.User);
            Assert.Contains(likes, x => x.Post == likeToAdd2.Post &&
                                        x.Comment == likeToAdd2.Comment &&
                                        x.LikeableType == likeToAdd2.LikeableType &&
                                        x.User == likeToAdd2.User);
        }

        [Fact]
        public async Task GetLikesFromComment()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "GetLikesFromComment@email.com", Password = "1234", UserName = "GetLikesFromComment" });
            var user2 = await _fixture.Db.Users.AddAsync(
                new User() { Email = "GetLikesFromComment2@email.com", Password = "1234", UserName = "GetLikesFromComment2" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "GetLikesFromComment" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post", Name = "GetLikesFromComment" });
            var comment = await _fixture.Db.Comments.AddAsync(new Comment()
                { Author = user.Entity, Content = "test", PostParent = post.Entity });
            _fixture.UnitOfWork.Save();
            var likeToAdd = new AddLikeDto()
                { LikeableType = LikeableType.Comment, Comment = comment.Entity.Id, User = user.Entity.Id };
            var likeToAdd2 = new AddLikeDto()
                { LikeableType = LikeableType.Comment, Comment = comment.Entity.Id, User = user2.Entity.Id };
            await _service.AddLike(likeToAdd);
            await _service.AddLike(likeToAdd2);

            // Act
            var likes = (await _service.GetLikesFromComment(comment.Entity.Id)).ToArray();

            // Assert
            Assert.Contains(likes, x => x.Post == likeToAdd.Post &&
                                        x.Comment == likeToAdd.Comment &&
                                        x.LikeableType == likeToAdd.LikeableType &&
                                        x.User == likeToAdd.User);
            Assert.Contains(likes, x => x.Post == likeToAdd2.Post &&
                                        x.Comment == likeToAdd2.Comment &&
                                        x.LikeableType == likeToAdd2.LikeableType &&
                                        x.User == likeToAdd2.User);
        }

        [Fact]
        public async Task GetLikesFromPost()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "GetLikesFromPost@email.com", Password = "1234", UserName = "GetLikesFromPost" });
            var user2 = await _fixture.Db.Users.AddAsync(
                new User() { Email = "GetLikesFromPost2@email.com", Password = "1234", UserName = "GetLikesFromPost2" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "GetLikesFromPost" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post", Name = "GetLikesFromPost" });
            _fixture.UnitOfWork.Save();
            var likeToAdd = new AddLikeDto()
            { LikeableType = LikeableType.Post, Post = post.Entity.Id, User = user.Entity.Id };
            var likeToAdd2 = new AddLikeDto()
            { LikeableType = LikeableType.Post, Post = post.Entity.Id, User = user2.Entity.Id };
            await _service.AddLike(likeToAdd);
            await _service.AddLike(likeToAdd2);

            // Act
            var likes = (await _service.GetLikesFromPost(post.Entity.Id)).ToArray();

            // Assert
            Assert.Contains(likes, x => x.Post == likeToAdd.Post &&
                                        x.Comment == likeToAdd.Comment &&
                                        x.LikeableType == likeToAdd.LikeableType &&
                                        x.User == likeToAdd.User);
            Assert.Contains(likes, x => x.Post == likeToAdd2.Post &&
                                        x.Comment == likeToAdd2.Comment &&
                                        x.LikeableType == likeToAdd2.LikeableType &&
                                        x.User == likeToAdd2.User);
        }

        [Fact]
        public async Task GetLikesFromUser()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "GetLikesFromUser@email.com", Password = "1234", UserName = "GetLikesFromUser" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "GetLikesFromUser" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post", Name = "GetLikesFromUser" });
            var comment = await _fixture.Db.Comments.AddAsync(new Comment()
            { Author = user.Entity, Content = "test", PostParent = post.Entity });
            _fixture.UnitOfWork.Save();
            var likeToAdd = new AddLikeDto()
            { LikeableType = LikeableType.Post, Post = post.Entity.Id, User = user.Entity.Id };
            var likeToAdd2 = new AddLikeDto()
            { LikeableType = LikeableType.Comment, Comment = comment.Entity.Id, User = user.Entity.Id };
            await _service.AddLike(likeToAdd);
            await _service.AddLike(likeToAdd2);

            // Act
            var likes = (await _service.GetLikesFromUser(user.Entity.Id)).ToArray();

            // Assert
            Assert.Contains(likes, x => x.Post == likeToAdd.Post &&
                                        x.Comment == likeToAdd.Comment &&
                                        x.LikeableType == likeToAdd.LikeableType &&
                                        x.User == likeToAdd.User);
            Assert.Contains(likes, x => x.Post == likeToAdd2.Post &&
                                        x.Comment == likeToAdd2.Comment &&
                                        x.LikeableType == likeToAdd2.LikeableType &&
                                        x.User == likeToAdd2.User);
        }

        [Fact]
        public async Task AddNullLike()
        {

            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.AddLike(null));
        }

        [Fact]
        public async Task UpdateNullLike()
        {

            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.UpdateLike(null));
        }
    }
}
