using System;
using AutoMapper;
using DbAccess.Data.POCO;
using DbAccess.Repositories.Comment;
using MyBlogAPI.DTO.Category;
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
            var config = new MapperConfiguration(cfg => { cfg.AddProfile(new AutoMapperProfile()); });
            var mapper = config.CreateMapper();
            _service = new MyBlogAPI.Services.CommentService.CommentService(new CommentRepository(_fixture.Db),
                mapper, _fixture.UnitOfWork);
        }

        [Fact]
        public async void AddComment()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() {EmailAddress = "AddComment@email.com", Password = "1234", Username = "ok"});
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
            Assert.Throws<ArgumentNullException>(() => _service.AddComment(comment).Result);
        }
    }
}
