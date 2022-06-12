using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlogCoreAPI.DTOs.Comment;
using BlogCoreAPI.Services.CommentService;
using DBAccess.Data.POCO;
using DBAccess.Repositories.Comment;
using DBAccess.Repositories.Post;
using DBAccess.Repositories.User;
using Xunit;

namespace BlogCoreAPI.Tests.Services
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
            _service = new BlogCoreAPI.Services.CommentService.CommentService(new CommentRepository(_fixture.Db),
                mapper, _fixture.UnitOfWork, new UserRepository(_fixture.Db, _fixture.UserManager), new PostRepository(_fixture.Db));
        }

        [Fact]
        public async Task AddComment()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() {Email = "AddComment@email.com", Password = "1234", UserName = "AddComment"});
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
        public async Task AddCommentWithCommentParent()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "AddComWithComParent@email.com", Password = "1234", UserName = "AddComWithComPa" });
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
        public async Task AddCommentWithInvalidCommentParent()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "AddComWithInvalidComPa@email.com", Password = "1234", UserName = "AddComWiInvComPa" });
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
        public async Task AddCommentWithNullContent()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "AddNullContentCategory@email.com", Password = "1234", UserName = "AddNullCom" });
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
        public async Task UpdateCommentWithSelfCommentParent()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "UpdateComWithSelfComPa@email.com", Password = "1234", UserName = "UpComWiSelfComPa" });
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
                Id = commentAdded.Id,
                Author = user.Entity.Id,
                CommentParent = commentAdded.Id,
                Content = "A new Content Updated",
                PostParent = post.Entity.Id
            }));
        }

        [Fact]
        public async Task AddCommentNotFoundParent()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "AddCommentNotFoundParent@email.com", Password = "1234", UserName = "AddBadParent" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "AddBadComPa" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post", Name = "AddBadParentPost" });
            _fixture.UnitOfWork.Save();
            var comment = new AddCommentDto() { Author = user.Entity.Id, PostParent = post.Entity.Id, Content = "AddCommentNotFoundParent", CommentParent = 654438};

            // Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.AddComment(comment));
        }

        [Fact]
        public async Task GetCommentNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.GetComment(685479));
        }

        [Fact]
        public async Task UpdateCommentOnlyOneProperty()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "UpdateCommentOnlyOneProperty@email.com", Password = "1234", UserName = "UptCmtOnlyOProp" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "UptCmtOnlyOProp" });
            var post = await _fixture.Db.Posts.AddAsync(new Post()
                {Author = user.Entity, Category = category.Entity, Content = "UpdateCommentOnlyOneProperty", Name = "UpdateCommentOnlyOneProperty" });

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
        public async Task UpdateCommentInvalid()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "UpdateCommentInvalid@email.com", Password = "1234", UserName = "UptCmtInvalid" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "UptCmtInvalid" });
            var post = await _fixture.Db.Posts.AddAsync(new Post()
                { Author = user.Entity, Category = category.Entity, Content = "UpdateCommentInvalid", Name = "UpdateCommentInvalid" });
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
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.UpdateComment(commentToUpdate));
        }

        [Fact]
        public async Task UpdateCommentMissingContent()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "UpdateCommentMissingContent@email.com", Password = "1234", UserName = "UptCmtMisContent" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "UptCmtMisContent" });
            var post = await _fixture.Db.Posts.AddAsync(new Post()
                { Author = user.Entity, Category = category.Entity, Content = "UptCmtMisContent", Name = "UpdateCommentMissingContent" });
            _fixture.UnitOfWork.Save();
            var comment = await _service.AddComment(new AddCommentDto()
            {
                Author = user.Entity.Id,
                Content = "UptCmtMisContent",
                PostParent = post.Entity.Id
            });
            var commentToUpdate = new UpdateCommentDto()
            {
                Id = comment.Id,
                Author = user.Entity.Id,
                PostParent = post.Entity.Id
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.UpdateComment(commentToUpdate));
        }

        [Fact]
        public async Task AddNullComment()
        {

            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.AddComment(null));
        }

        [Fact]
        public async Task UpdateNullComment()
        {

            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.UpdateComment(null));
        }

        [Fact]
        public async Task AddCommentWithNullAuthor()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "AddCommentWithNullAuthor@email.com", Password = "1234", UserName = "AddComWithNullA" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "AddComWithNullA" });
            var post = await _fixture.Db.Posts.AddAsync(new Post()
                { Author = user.Entity, Category = category.Entity, Content = "AddComWithNullA", Name = "AddCommentWithNullAuthor" });
            _fixture.UnitOfWork.Save();
            var commentToAdd = new AddCommentDto()
            {
                Content = "AddComWithNullA",
                PostParent = post.Entity.Id
            };

            // Act && Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.AddComment(commentToAdd));
        }

        [Fact]
        public async Task AddCommentWithNullPostParent()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "AddCommentWithNullPostParent@email.com", Password = "1234", UserName = "AddComWithNullP" });
            _fixture.UnitOfWork.Save();
            var commentToAdd = new AddCommentDto()
            {
                Content = "AddComWithNullP",
                Author = user.Entity.Id
            };

            // Act && Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.AddComment(commentToAdd));
        }

        [Fact]
        public async Task GetComment()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "GetComment@email.com", Password = "1234", UserName = "GetComment" });
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
        public async Task UpdateComment()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "UpdateComment@email.com", Password = "1234", UserName = "UpdateComment" });
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
            Assert.True(commentDb.Id == commentToUpdate.Id &&
                        commentDb.Author == commentToUpdate.Author &&
                        commentDb.Content == commentToUpdate.Content &&
                        commentDb.CommentParent == commentToUpdate.CommentParent &&
                        commentDb.PostParent == commentToUpdate.PostParent);
        }

        [Fact]
        public async Task UpdateCommentNotFound()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "UpdateComment@email.com", Password = "1234", UserName = "UpdateComment" });
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
        public async Task UpdateCommentWithSameExistingProperties()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "UpdateComWithSameExistingProp@email.com", Password = "1234", UserName = "UpComSaExtProp" });
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
            Assert.True(commentDb.Id == commentToUpdate.Id &&
                        commentDb.Author == commentToUpdate.Author &&
                        commentDb.Content == commentToUpdate.Content &&
                        commentDb.CommentParent == commentToUpdate.CommentParent &&
                        commentDb.PostParent == commentToUpdate.PostParent);
        }

        [Fact]
        public async Task DeleteComment()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "DeleteComment@email.com", Password = "1234", UserName = "DeleteComment" });
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
        public async Task DeleteCommentNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.DeleteComment(175574));
        }

        [Fact]
        public async Task GetAllComments()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "GetAllComments@email.com", Password = "1234", UserName = "GetAllComments" });
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

        [Fact]
        public async Task GetCommentsFromPost()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "GetCommentsFromPost@email.com", Password = "1234", UserName = "GetCommentsFromPost" });
            var user2 = await _fixture.Db.Users.AddAsync(
                new User() { Email = "GetCommentsFromPost2@email.com", Password = "1234", UserName = "GetCommentsFromPost2" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "GetCommentsFromPost" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post", Name = "GetCommentsFromPost" });
            _fixture.UnitOfWork.Save();
            var comment = new AddCommentDto() { Author = user.Entity.Id, Content = "A new Content", PostParent = post.Entity.Id };
            var commentAdded = await _service.AddComment(comment);
            comment.Content = "A new Content 2";
            comment.Author = user2.Entity.Id;
            var commentAdded2 = await _service.AddComment(comment);

            // Act
            var comments = (await _service.GetCommentsFromPost(post.Entity.Id)).ToArray();

            // Assert
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

        [Fact]
        public async Task GetCommentsFromUser()
        {
            // Arrange
            var user = await _fixture.Db.Users.AddAsync(
                new User() { Email = "GetCommentsFromUser@email.com", Password = "1234", UserName = "GetCommentsFromUser" });
            var category = await _fixture.Db.Categories.AddAsync(
                new Category() { Name = "GetCommentsFromUser" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post", Name = "GetCommentsFromUser" });
            var post2 = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = category.Entity, Content = "new post", Name = "GetCommentsFromUser2" });
            _fixture.UnitOfWork.Save();
            var comment = new AddCommentDto() { Author = user.Entity.Id, Content = "A new Content", PostParent = post.Entity.Id };
            var commentAdded = await _service.AddComment(comment);
            comment.PostParent = post2.Entity.Id;
            var commentAdded2 = await _service.AddComment(comment);

            // Act
            var comments = (await _service.GetCommentsFromUser(user.Entity.Id)).ToArray();

            // Assert
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
