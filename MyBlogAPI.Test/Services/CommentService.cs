using System;
using AutoMapper;
using DbAccess.Data.POCO;
using DbAccess.Repositories.Comment;
using DbAccess.Repositories.Post;
using DbAccess.Repositories.User;
using MyBlogAPI.DTO.Comment;
using MyBlogAPI.Services.CommentService;
using Xunit;

namespace MyBlogAPI.Test.Services
{
    public class CommentService : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;
        private readonly ICommentService _service;

        public CommentService(DatabaseFixture databaseFixture)
        {
            _fixture = databaseFixture;
            var config = new MapperConfiguration(cfg => { cfg.AddProfile(databaseFixture.MapperProfile); });
            var mapper = config.CreateMapper();
            _service = new MyBlogAPI.Services.CommentService.CommentService(new CommentRepository(_fixture.Db),
                mapper, _fixture.UnitOfWork, new UserRepository(_fixture.Db), new PostRepository(_fixture.Db));
        }

        [Fact]
        public async void AddComment()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() {EmailAddress = "AddComment@email.com", Password = "1234", Username = "AddComment"});
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() {Name = "AddCommentName"});
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() {Author = user.Entity, Category = category.Entity, Content = "new post", Name = "AddCommentName"});
            _fixture.UnitOfWork.Save();
            var comment = new AddCommentDto() { Author = user.Entity.Id, Content = "A new Content", PostParent = post.Entity.Id };

            // Act
            var commentAdded = await _service.AddComment(comment);

            // Assert
            Assert.Contains(await _service.GetAllComments(), x => x.Id == commentAdded.Id &&
                                                                  x.Author == comment.Author &&
                                                                  x.Content == comment.Content &&
                                                                  x.CommentParent == comment.CommentParent &&
                                                                  x.PostParent == comment.PostParent);
        }

        [Fact]
        public async void AddCommentWithNullContent()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "AddNullContentCategory@email.com", Password = "1234", Username = "AddNullCom" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "AddNulContentComment" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post", Name = "AddCategoryPost" });
            _fixture.UnitOfWork.Save();
            var comment = new AddCommentDto() { Author = user.Entity.Id, PostParent = post.Entity.Id };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.AddComment(comment));
        }

        [Fact]
        public async void AddCommentNotFoundParent()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "AddCommentNotFoundParent@email.com", Password = "1234", Username = "AddBadParent" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "AddBadComPa" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post", Name = "AddBadParentPost" });
            _fixture.UnitOfWork.Save();
            var comment = new AddCommentDto() { Author = user.Entity.Id, PostParent = post.Entity.Id, CommentParent = 654438};

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.AddComment(comment));
        }

        [Fact]
        public async void GetCommentNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.GetComment(685479));
        }

        [Fact]
        public async void UpdateCommentOnlyOneProperty()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "UpdateCommentOnlyOneProperty@email.com", Password = "1234", Username = "UptCmtOnlyOProp" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "UptCmtOnlyOProp" }); ;
            var post = await _fixture.Db.Posts.AddAsync(new Post()
                {Author = user.Entity, Category = category.Entity, Content = "UpdateCommentOnlyOneProperty"});

            _fixture.UnitOfWork.Save();
            var comment = await _service.AddComment(new AddCommentDto()
            {
                Author = user.Entity.Id,
                Content = "UpdateCmtOnlyOneProperty",
                PostParent = post.Entity.Id
            });
            var commentToUpdate = new UpdateCommentDto()
            {
                Id = comment.Id,
                Author = user.Entity.Id,
                Content = "UpdateCmtOnlyOneProperty Here",
                PostParent = post.Entity.Id
            };

            // Act
            await _service.UpdateComment(commentToUpdate);

            // Assert
            var commentUpdated = await _service.GetComment(comment.Id);
            Assert.True(commentToUpdate.Content == commentUpdated.Content &&
                        commentToUpdate.Author == commentUpdated.Author &&
                        commentToUpdate.PostParent == commentUpdated.PostParent);
        }

        [Fact]
        public async void UpdateCommentInvalid()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "UpdateCommentInvalid@email.com", Password = "1234", Username = "UptCmtInvalid" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "UptCmtInvalid" }); ;
            var post = await _fixture.Db.Posts.AddAsync(new Post()
                { Author = user.Entity, Category = category.Entity, Content = "UpdateCommentInvalid" });

            _fixture.UnitOfWork.Save();
            var comment = await _service.AddComment(new AddCommentDto()
            {
                Author = user.Entity.Id,
                Content = "UpdateCmtInvalid",
                PostParent = post.Entity.Id
            });
            var commentToUpdate = new UpdateCommentDto()
            {
                Id = comment.Id,
                Content = "UpdateCmtInvalid",
                PostParent = post.Entity.Id
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.UpdateComment(commentToUpdate));
        }
    }
}
