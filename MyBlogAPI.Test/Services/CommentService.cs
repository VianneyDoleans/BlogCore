using System;
using System.Linq;
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
        public async void AddCommentWithCommentParent()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "AddComWithComParent@email.com", Password = "1234", Username = "AddComWithComPa" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "AddComWithComPa" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post", Name = "AddComWithComPa" });
            _fixture.UnitOfWork.Save();
            var commentParent = new AddCommentDto() { Author = user.Entity.Id, Content = "A new Content", PostParent = post.Entity.Id };
            var commentParentAdded = await _service.AddComment(commentParent);
            var commentChild = new AddCommentDto()
            {
                Author = user.Entity.Id,
                CommentParent = commentParentAdded.Id,
                Content = "A new content child",
                PostParent = post.Entity.Id
            };

            // Act
            var commentChildAdded = await _service.AddComment(commentChild);

            // Assert
            Assert.Contains(await _service.GetAllComments(), x => x.Id == commentChildAdded.Id &&
                                                                  x.Author == commentChild.Author &&
                                                                  x.Content == commentChild.Content &&
                                                                  x.CommentParent == commentChild.CommentParent &&
                                                                  x.PostParent == commentChild.PostParent);
        }

        [Fact]
        public async void AddCommentWithInvalidCommentParent()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "AddComWithInvalidComPa@email.com", Password = "1234", Username = "AddComWiInvComPa" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "AddComWiInvComPa" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post", Name = "AddComWiInvComPa" });
            _fixture.UnitOfWork.Save();
            var commentChild = new AddCommentDto()
            {
                Author = user.Entity.Id,
                CommentParent = 165476,
                Content = "A new content child",
                PostParent = post.Entity.Id
            };

            // Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.AddComment(commentChild));
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
        public async void UpdateCommentWithSelfCommentParent()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "UpdateComWithSelfComPa@email.com", Password = "1234", Username = "UpComWiSelfComPa" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "UpComWiSelfComPa" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post", Name = "UpComWiSelfComPa" });
            _fixture.UnitOfWork.Save();
            var comment = new AddCommentDto() { Author = user.Entity.Id, Content = "A new Content", PostParent = post.Entity.Id };
            var commentAdded = await _service.AddComment(comment);

            //Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.UpdateComment(new UpdateCommentDto()
            {
                Author = user.Entity.Id,
                CommentParent = commentAdded.Id,
                Content = "A new Content Updated",
                PostParent = post.Entity.Id
            }));
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
                new Category() { Name = "UptCmtOnlyOProp" });
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
                new Category() { Name = "UptCmtInvalid" });
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

        [Fact]
        public async void AddNullComment()
        {

            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.AddComment(null));
        }

        [Fact]
        public async void UpdateNullComment()
        {

            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.UpdateComment(null));
        }

        [Fact]
        public async void AddCommentWithNullAuthor()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "AddCommentWithNullAuthor@email.com", Password = "1234", Username = "AddComWithNullA" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "AddComWithNullA" });
            var post = await _fixture.Db.Posts.AddAsync(new Post()
                { Author = user.Entity, Category = category.Entity, Content = "AddComWithNullA" });
            _fixture.UnitOfWork.Save();
            var commentToAdd = new AddCommentDto()
            {
                Content = "AddComWithNullA",
                PostParent = post.Entity.Id
            };

            // Act && Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.AddComment(commentToAdd));
        }

        [Fact]
        public async void AddCommentWithNullPostParent()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "AddCommentWithNullPostParent@email.com", Password = "1234", Username = "AddComWithNullP" });
            _fixture.UnitOfWork.Save();
            var commentToAdd = new AddCommentDto()
            {
                Content = "AddComWithNullP",
                Author = user.Entity.Id
            };

            // Act && Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.AddComment(commentToAdd));
        }

        [Fact]
        public async void GetComment()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "GetComment@email.com", Password = "1234", Username = "GetComment" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "GetComment" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post", Name = "GetComment" });
            _fixture.UnitOfWork.Save();
            var comment = new AddCommentDto() { Author = user.Entity.Id, Content = "A new Content", PostParent = post.Entity.Id };
            var commentAdded = await _service.AddComment(comment);

            // Act
            var commentDb = await _service.GetComment(commentAdded.Id);

            // Assert
            Assert.True(commentDb.Id == commentAdded.Id && 
                        commentDb.Author == comment.Author &&
                        commentDb.Content == comment.Content &&
                        commentDb.CommentParent == comment.CommentParent &&
                        commentDb.PostParent == comment.PostParent);
        }

        [Fact]
        public async void UpdateComment()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "UpdateComment@email.com", Password = "1234", Username = "UpdateComment" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "UpdateComment" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post", Name = "UpdateComment" });
            _fixture.UnitOfWork.Save();
            var comment = new AddCommentDto() { Author = user.Entity.Id, Content = "A new Content", PostParent = post.Entity.Id };
            var commentAdded = await _service.AddComment(comment);
            var commentToUpdate = new UpdateCommentDto() { Id = commentAdded.Id, Author = user.Entity.Id, Content = "A new Content Updated", PostParent = post.Entity.Id };

            // Act
            await _service.UpdateComment(commentToUpdate);

            // Assert
            var commentDb = await _service.GetComment(commentAdded.Id);
            Assert.True(commentDb.Id == commentAdded.Id &&
                        commentDb.Author == comment.Author &&
                        commentDb.Content == comment.Content &&
                        commentDb.CommentParent == comment.CommentParent &&
                        commentDb.PostParent == comment.PostParent);
        }

        [Fact]
        public async void UpdateCommentNotFound()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "UpdateComment@email.com", Password = "1234", Username = "UpdateComment" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "UpdateComment" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post", Name = "UpdateComment" });
            _fixture.UnitOfWork.Save();
            var commentToUpdate = new UpdateCommentDto()
                {Id = 164854, Content = "ok", Author = user.Entity.Id, PostParent = post.Entity.Id};

            // Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.UpdateComment(commentToUpdate));
        }

        [Fact]
        public async void UpdateCommentWithSameExistingProperties()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "UpdateComWithSameExistingProp@email.com", Password = "1234", Username = "UpComSaExtProp" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "UpComSaExtProp" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post", Name = "UpComSaExtProp" });
            _fixture.UnitOfWork.Save();
            var comment = new AddCommentDto() { Author = user.Entity.Id, Content = "A new Content", PostParent = post.Entity.Id };
            var commentAdded = await _service.AddComment(comment);
            var commentToUpdate = new UpdateCommentDto() { Id = commentAdded.Id, Author = user.Entity.Id, Content = "A new Content", PostParent = post.Entity.Id };

            // Act
            await _service.UpdateComment(commentToUpdate);

            // Assert
            var commentDb = await _service.GetComment(commentAdded.Id);
            Assert.True(commentDb.Id == commentAdded.Id &&
                        commentDb.Author == comment.Author &&
                        commentDb.Content == comment.Content &&
                        commentDb.CommentParent == comment.CommentParent &&
                        commentDb.PostParent == comment.PostParent);
        }

        [Fact]
        public async void DeleteComment()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "DeleteComment@email.com", Password = "1234", Username = "DeleteComment" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "DeleteComment" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post", Name = "DeleteComment" });
            _fixture.UnitOfWork.Save();
            var comment = new AddCommentDto() { Author = user.Entity.Id, Content = "A new Content", PostParent = post.Entity.Id };
            var commentAdded = await _service.AddComment(comment);

            // Act
            await _service.DeleteComment(commentAdded.Id);

            // Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.GetComment(commentAdded.Id));
        }

        [Fact]
        public async void DeleteCommentNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.DeleteComment(175574));
        }

        [Fact]
        public async void GetAllComments()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "GetAllComments@email.com", Password = "1234", Username = "GetAllComments" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "GetAllComments" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post", Name = "GetAllComments" });
            var post2 = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post2", Name = "GetAllComments2" });
            _fixture.UnitOfWork.Save();
            var comment = new AddCommentDto() { Author = user.Entity.Id, Content = "A new Content", PostParent = post.Entity.Id };
            var commentAdded = await _service.AddComment(comment);
            comment.PostParent = post2.Entity.Id;
            var commentAdded2 = await _service.AddComment(comment);


            // Act
            var comments = (await _service.GetAllComments()).ToArray();

            // Assert
            Assert.Contains(comments, x => x.Id == commentAdded.Id &&
                                           x.Author == commentAdded.Author &&
                                           x.Content == commentAdded.Content &&
                                           x.CommentParent == commentAdded.CommentParent &&
                                           x.PostParent == commentAdded.PostParent);
            Assert.Contains(comments, x => x.Id == commentAdded2.Id &&
                                           x.Author == commentAdded2.Author &&
                                           x.Content == commentAdded2.Content &&
                                           x.CommentParent == commentAdded2.CommentParent &&
                                           x.PostParent == commentAdded2.PostParent);
        }
    }
}
