using System;
using System.Linq;
using AutoMapper;
using DbAccess.Data.POCO;
using DbAccess.Repositories.Comment;
using DbAccess.Repositories.Like;
using DbAccess.Repositories.Post;
using DbAccess.Repositories.User;
using MyBlogAPI.DTO.Like;
using MyBlogAPI.Services.LikeService;
using Xunit;

namespace MyBlogAPI.Tests.Services
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
        public async void AddLikeWithNullLikeableElementPost()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "AddLikeNullElementPost@email.com", Password = "1234", Username = "AddLikeNullEP" });
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
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.AddLike(like));
        }

        [Fact]
        public async void AddLikeWithNullLikeableElementComment()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "AddLikeNullElementC@email.com", Password = "1234", Username = "AddLikeNullEC" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "AddLikeNullEC" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post", Name = "AddLikeNullEC" });
            _fixture.UnitOfWork.Save();
            var like = new AddLikeDto()
                { LikeableType = LikeableType.Comment, User = user.Entity.Id, Post = post.Entity.Id };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.AddLike(like));
        }

        [Fact]
        public async void AddLikeWithTwoLikeableElement()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "AddLikeTwElement@email.com", Password = "1234", Username = "AddLikeTwE" });
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
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.AddLike(like));
        }

        [Fact]
        public async void AddLikeWithNullUser()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "AddLikeNullU@email.com", Password = "1234", Username = "AddLikeNullU" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "AddLikeNullU" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post", Name = "AddLikeNullU" });
            _fixture.UnitOfWork.Save();
            var like = new AddLikeDto()
                { LikeableType = LikeableType.Post, Post = post.Entity.Id };

            // Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.AddLike(like));
        }

        [Fact]
        public async void AddLikeWithOnlyLikeableType()
        {
            // Arrange
            var like = new AddLikeDto()
                { LikeableType = LikeableType.Post };

            // Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.AddLike(like));
        }

        [Fact]
        public async void GetLike()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() {EmailAddress = "GetLike@email.com", Password = "1234", Username = "GetLike" });
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
        public async void GetLikeNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.GetLike(685479));
        }

        [Fact]
        public async void UpdateLike()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "UpdateLike@email.com", Password = "1234", Username = "UpdateLike" });
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
        public async void UpdateLikeNotFound()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "UpdateLikeNotFound@email.com", Password = "1234", Username = "UpdateLikeNotFound" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "UpdateLikeNotFound" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post", Name = "UpdateLikeNotFound" });
            _fixture.UnitOfWork.Save();

            // Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.UpdateLike(new UpdateLikeDto()
            { Id = 164854, LikeableType = LikeableType.Post, Post = post.Entity.Id, User = user.Entity.Id}));
        }

        [Fact]
        public async void UpdateLikeWithSameExistingProperties()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "UpdateLikeWithSameExistingPropertie@email.com", Password = "1234", Username = "UpLikeWithSmExtPro" });
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
        public async void UpdateLikeInvalid()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "UpdateLikeInvalid@email.com", Password = "1234", Username = "UpdateLikeInvalid" });
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
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.UpdateLike(likeToUpdate));
        }

        [Fact]
        public async void DeleteLike()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "DeleteLike@email.com", Password = "1234", Username = "DeleteLike" });
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
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.GetLike(like.Id));
        }

        [Fact]
        public async void DeleteLikeNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.DeleteLike(175574));
        }

        [Fact]
        public async void GetAllLikes()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "GetAllLikes@email.com", Password = "1234", Username = "GetAllLikes" });
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
        public async void GetLikesFromComment()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "GetLikesFromComment@email.com", Password = "1234", Username = "GetLikesFromComment" });
            var user2 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "GetLikesFromComment2@email.com", Password = "1234", Username = "GetLikesFromComment2" });
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
        public async void GetLikesFromPost()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "GetLikesFromPost@email.com", Password = "1234", Username = "GetLikesFromPost" });
            var user2 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "GetLikesFromPost2@email.com", Password = "1234", Username = "GetLikesFromPost2" });
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
        public async void GetLikesFromUser()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "GetLikesFromUser@email.com", Password = "1234", Username = "GetLikesFromUser" });
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
        public async void AddNullLike()
        {

            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.AddLike(null));
        }

        [Fact]
        public async void UpdateNullLike()
        {

            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.UpdateLike(null));
        }
    }
}
