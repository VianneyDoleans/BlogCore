using System;
using System.Collections.Generic;
using System.Linq;
using DbAccess.Data.POCO;
using DbAccess.Specifications;
using DbAccess.Specifications.FilterSpecifications.Filters;
using DbAccess.Specifications.SortSpecification;
using Xunit;

namespace DBAccess.Tests.Repositories
{
    public class CommentRepository
    {
        private readonly DatabaseFixture _fixture;

        public CommentRepository()
        {
            _fixture = new DatabaseFixture();
        }

        [Fact]
        public async void AddCommentAsync()
        {
            // Arrange
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "AddCommentAsync@email.com", Password = "1234", UserName = "AddCommentAsync" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "AddCommentAsync" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "AddCommentAsync", Content = "AddCommentAsync", Category = category.Entity });
            var testComment = new Comment()
                {Author = await _fixture.Db.Users.FindAsync(1), PostParent = post.Entity, Content = "AddCommentAsync" };

            // Act
            await commentRepository.AddAsync(testComment);
            _fixture.UnitOfWork.Save();

            // Assert
            Assert.True(_fixture.Db.Comments.First(x => x.Content == "AddCommentAsync") != null);
        }

        [Fact]
        public async void AddNullCommentAsync()
        {
            // Arrange
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await commentRepository.AddAsync(null));
        }

        [Fact]
        public async void GetCommentAsync()
        {
            // Arrange
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);
            var user = _fixture.Db.Users.Add(
                new User() { EmailAddress = "GetCommentAsync@email.com", Password = "1234", UserName = "GetCommentAsync" });
            var category = _fixture.Db.Categories.Add(new Category() { Name = "GetCommentAsync" });
            var post = _fixture.Db.Posts.Add(
                new Post() { Author = user.Entity, Name = "GetCommentAsync", Content = "GetCommentAsync", Category = category.Entity });
            var testComment = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "GetCommentAsync"
            };
            commentRepository.Add(testComment);
            _fixture.UnitOfWork.Save();

            // Act
            var result = await commentRepository.GetAsync(testComment.Id);

            // Assert
            Assert.True(result == await _fixture.Db.Comments.FindAsync(testComment.Id));
        }

        [Fact]
        public async void GetCommentOutOfRangeAsync()
        {
            // Arrange
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);

            // Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await commentRepository.GetAsync(100));
        }

        [Fact]
        public async void GetAllAsync()
        {
            // Arrange
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);

            // Act
            var result = await commentRepository.GetAllAsync();

            // Assert
            Assert.True(result.Count() == _fixture.Db.Comments.Count());
        }

        [Fact]
        public async void RemoveAsync()
        {
            // Arrange
            var nbCommentsAtBeginning = _fixture.Db.Comments.Count();
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "ComRemoveAsync@email.com", Password = "1234", UserName = "ComRemoveAsync" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "ComRemoveAsync" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "ComRemoveAsync", Content = "ComRemoveAsync", Category = category.Entity });
            var testComment = new Comment()
                {Author = await _fixture.Db.Users.FindAsync(1), PostParent = post.Entity, Content = "ComRemoveAsync" };

            await commentRepository.AddAsync(testComment);
            _fixture.UnitOfWork.Save();
            var nbCommentAfterAdded = _fixture.Db.Comments.Count();

            // Act
            await commentRepository.RemoveAsync(testComment);
            _fixture.UnitOfWork.Save();
            var nbCommentAfterRemoved = _fixture.Db.Comments.Count();

            // Assert
            Assert.True(nbCommentsAtBeginning + 1 == nbCommentAfterAdded &&
                        nbCommentAfterRemoved == nbCommentsAtBeginning);
        }

        [Fact]
        public async void RemoveNullAsync()
        {
            // Arrange
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);

            // Act & Asser
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await commentRepository.RemoveAsync(null));
        }

        [Fact]
        public void AddComment()
        {
            // Arrange
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);
            var user = _fixture.Db.Users.Add(
                new User() { EmailAddress = "AddComment@email.com", Password = "1234", UserName = "AddComment" });
            var category = _fixture.Db.Categories.Add(new Category() { Name = "AddComment" });
            var post = _fixture.Db.Posts.Add(
                new Post() { Author = user.Entity, Name = "AddComment", Content = "AddComment", Category = category.Entity });
            _fixture.Db.Users.Add(user.Entity);
            _fixture.Db.SaveChanges();
            var testComment = new Comment() { Author = user.Entity, PostParent = post.Entity, Content = "AddComment" };

            // Act
            commentRepository.Add(testComment);
            _fixture.UnitOfWork.Save();

            // Assert
            Assert.True(_fixture.Db.Comments.First(x => x.Content == "AddComment") != null);
        }

        [Fact]
        public void AddNullComment()
        {
            // Arrange
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                commentRepository.Add(null);
                _fixture.Db.SaveChanges();
            });
        }

        [Fact]
        public void CountAll()
        {
            // Arrange
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);

            // Act & Assert
            Assert.True(_fixture.Db.Comments.Count() == commentRepository.CountAll());
        }

        [Fact]
        public async void CountAllAsync()
        {
            // Arrange
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);

            // Act & Assert
            Assert.True(_fixture.Db.Comments.Count() == await commentRepository.CountAllAsync());
        }


        [Fact]
        public void GetAll()
        {
            // Arrange
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);

            // Act
            var result = commentRepository.GetAll();

            // Assert
            Assert.True(result.Count() == _fixture.Db.Comments.Count());
        }

        [Fact]
        public async void GetAsyncSpecificationBasic()
        {
            // Arrange
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);
            var user = await _fixture.Db.Users.AddAsync(
               new User() { EmailAddress = "GetASpecBasicCo@email.com", Password = "1234", UserName = "GetASpecBasicCo" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "GetASpecBasicCo" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "GetASpecBasicCo", Content = "GetASpecBasicCo", Category = category.Entity });
            _fixture.Db.Users.Add(user.Entity);
            _fixture.Db.SaveChanges();
            var testComment = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "GetAsyncSpecification"
            };
            var testComment2 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "GetAsyncSpecification2"
            };
            var testComment3 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "GetAsyncSpecification3"
            };
            var testComment4 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "GetAsyncSpecification4"
            };
            commentRepository.Add(testComment);
            commentRepository.Add(testComment2);
            commentRepository.Add(testComment3);
            commentRepository.Add(testComment4);
            _fixture.UnitOfWork.Save();

            // Act
            var result = await commentRepository.GetAsync(new IdSpecification<Comment>(testComment2.Id));

            // Assert
            Assert.True(result.First().Id == testComment2.Id);
        }

        [Fact]
        public async void GetAsyncWithTwoSpecifications()
        {
            // Arrange
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);
            var user = await _fixture.Db.Users.AddAsync(
               new User() { EmailAddress = "GetAWiTwoSpecCo@email.com", Password = "1234", UserName = "GetAWiTwoSpecCo" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "GetAWiTwoSpecCo" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "GetAWiTwoSpecCo", Content = "GetAWiTwoSpecCo", Category = category.Entity });
            var testComment = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncABSpecification"
            };
            var testComment2 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncAUSpecification2"
            };
            var testComment3 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncAKSpecification3"
            };
            var testComment5 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncAKSpecification3164"
            };
            var testComment6 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncAKSpecification32"
            };
            var testComment4 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncAWSpecification4"
            };
            await commentRepository.AddAsync(testComment);
            await commentRepository.AddAsync(testComment2);
            await commentRepository.AddAsync(testComment3);
            await commentRepository.AddAsync(testComment4);
            await commentRepository.AddAsync(testComment5);
            await commentRepository.AddAsync(testComment6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await commentRepository.GetAsync(new ContentContainsSpecification<Comment>("AK") & new ContentContainsSpecification<Comment>("164"))).ToList();

            // Assert
            Assert.True(result.Count() == 1 && result.First().Content == testComment5.Content);
        }

        [Fact]
        public async void GetAsyncWithTwoSortsAndTwoSpecificationsAndPagination()
        {
            // Arrange
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);

            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "CommentTwoSortsAndTwoSpec@email.com", Password = "1234", UserName = "TwoSortsAndTwoSpecCo" });
            var user2 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "CommentAnotherTwoSortsAndTwoSpec@email.com", Password = "1234", UserName = "TwoSortsAndTwoSpecCo2" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "TwoSortsAndTwoSpecCo" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "CommentTwoSortsAndTwoSpec", Content = "TwoSortsAndTwoSpecComment", Category = category.Entity });
            await _fixture.Db.Comments.AddAsync(
                new Comment() { Author = user.Entity, PostParent = post.Entity, Content = "TwwooGetAsyncWithTwoSortsComment" });
            await _fixture.Db.Comments.AddAsync(
                new Comment() { Author = user2.Entity, PostParent = post.Entity, Content = "GetAsyncWithTwoSorts2Comment" });
            await _fixture.Db.Comments.AddAsync(
                new Comment() { Author = user.Entity, PostParent = post.Entity, Content = "GetAsyncWithTwoSorts3TwwooComment" });
            var comment4 = await _fixture.Db.Comments.AddAsync(
                new Comment() { Author = user2.Entity, PostParent = post.Entity, Content = "AGetTwwooAsyncWithTwoSorts4Comment" });
            await _fixture.Db.Comments.AddAsync(
                new Comment() { Author = user2.Entity, PostParent = post.Entity, Content = "GetAsyncTwwooWithTwoSorts5Comment" });
            await _fixture.Db.Comments.AddAsync(
                new Comment() { Author = user.Entity, PostParent = post.Entity, Content = "GetAsyncWithTwoSorts6Comment" });
            await _fixture.Db.Comments.AddAsync(
                new Comment() { Author = user.Entity, PostParent = post.Entity, Content = "TwwooGetAsyncWithTwoorts7Comment" });
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await commentRepository.GetAsync(new ContentContainsSpecification<Comment>("WithTwoSorts")
                                                            & new ContentContainsSpecification<Comment>("Twwoo"), new PagingSpecification(0, 20),
                new SortSpecification<Comment>(new OrderBySpecification<Comment>(x => x.Author.UserName), SortingDirectionSpecification.Descending) &
                new SortSpecification<Comment>(new OrderBySpecification<Comment>(x => x.Content), SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count() == 4 && result.First().Content == comment4.Entity.Content);
        }

        [Fact]
        public async void GetAsyncWithNoArgument()
        {
            // Arrange
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);

            // Act & Assert
            Assert.True((await commentRepository.GetAsync()).ToList().Count() == _fixture.Db.Comments.Count());
        }

        [Fact]
        public async void GetAsyncWithAllArguments()
        {
            // Arrange
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "CommentGetAsyncWithAllArguments@email.com", Password = "1234", UserName = "ComGetAWithAllArgs" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "ComGetAWithAllArgs" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "ComGetAWithAllArgs", Content = "ComGetAWithAllArgs", Category = category.Entity });
            var testComment = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncABSpecificationWithAllArguments"
            };
            var testComment2 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncAUSpecification2WithAllArguments"
            };
            var testComment3 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncAKSpecification3WithAllArguments"
            };
            var testComment4 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncAKSpecification3WithAllArguments163"
            };
            var testComment5 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncAKSpecification3WithAllArguments2"
            };
            var testComment6 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncAWSpecification4WithAllArguments"
            };
            await commentRepository.AddAsync(testComment);
            await commentRepository.AddAsync(testComment2);
            await commentRepository.AddAsync(testComment3);
            await commentRepository.AddAsync(testComment4);
            await commentRepository.AddAsync(testComment5);
            await commentRepository.AddAsync(testComment6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await commentRepository.GetAsync(new ContentContainsSpecification<Comment>("AK") &
                new ContentContainsSpecification<Comment>("WithAllArguments"),
                new PagingSpecification(0, 2),
                new SortSpecification<Comment>(
                    new OrderBySpecification<Comment>(x => x.Content),
                    SortingDirectionSpecification.Descending))).ToList();

            // Assert
            Assert.True(result.Count() == 2 && result.First().Content == testComment5.Content);
        }

        [Fact]
        public async void GetAsyncWithPagination()
        {
            // Arrange
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "CommentGetAsyncWithPagination@email.com", Password = "1234", UserName = "GetAsyncWithPagination" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "GetAsyncWithPagination" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "GetAsyncWithPagination", Content = "GetAsyncWithPagination", Category = category.Entity });
            var testComment = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "Comment1GetAsyncWithPagination1"
            };
            var testComment2 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "Comment1GetAsyncWithPagination2"
            };
            var testComment3 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "Comment1GetAsyncWithPagination3"
            };
            var testComment4 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "Comment1GetAsyncWithPagination4"
            };
            var testComment5 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "Comment1GetAsyncWithPagination5"
            };
            var testComment6 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "Comment1GetAsyncWithPagination6"
            };
            await commentRepository.AddAsync(testComment);
            await commentRepository.AddAsync(testComment2);
            await commentRepository.AddAsync(testComment3);
            await commentRepository.AddAsync(testComment4);
            await commentRepository.AddAsync(testComment5);
            await commentRepository.AddAsync(testComment6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await commentRepository.GetAsync(new ContentContainsSpecification<Comment>("1GetAsyncWithPagination"),
                new PagingSpecification(2, 3),
                new SortSpecification<Comment>(
                    new OrderBySpecification<Comment>(x => x.Content),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count() == 3);
            Assert.Contains(result, x => x.Id == testComment3.Id &&
                                         x.Content == testComment3.Content);
            Assert.Contains(result, x => x.Id == testComment4.Id &&
                                         x.Content == testComment4.Content);
            Assert.Contains(result, x => x.Id == testComment5.Id &&
                                         x.Content == testComment5.Content);
        }

        [Fact]
        public async void GetAsyncWithPaginationTakeOutOfRange()
        {
            // Arrange
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "CoGetAWithPagTaOfRg@email.com", Password = "1234", UserName = "CoGetAWithPagTaOfRg" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "CoGetAWithPagTaOfRg" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "CoGetAWithPagTaOfRg", Content = "CoGetAWithPagTaOfRg", Category = category.Entity });
            var testComment = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncWithPaginationTakeOutOfRange1"
            };
            var testComment2 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncWithPaginationTakeOutOfRange2"
            };
            var testComment3 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncWithPaginationTakeOutOfRange3"
            };
            var testComment5 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncWithPaginationTakeOutOfRange4"
            };
            var testComment6 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncWithPaginationTakeOutOfRange5"
            };
            var testComment4 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncWithPaginationTakeOutOfRange6"
            };
            await commentRepository.AddAsync(testComment);
            await commentRepository.AddAsync(testComment2);
            await commentRepository.AddAsync(testComment3);
            await commentRepository.AddAsync(testComment4);
            await commentRepository.AddAsync(testComment5);
            await commentRepository.AddAsync(testComment6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await commentRepository.GetAsync(new ContentContainsSpecification<Comment>("GetAsyncWithPaginationTakeOutOfRange"),
                new PagingSpecification(2, 22),
                new SortSpecification<Comment>(
                    new OrderBySpecification<Comment>(x => x.Content),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count() == 4);
            Assert.Contains(result, x => x.Id == testComment3.Id &&
                                         x.Content == testComment3.Content);
            Assert.Contains(result, x => x.Id == testComment4.Id &&
                                         x.Content == testComment4.Content);
            Assert.Contains(result, x => x.Id == testComment5.Id &&
                                         x.Content == testComment5.Content);
            Assert.Contains(result, x => x.Id == testComment6.Id &&
                                         x.Content == testComment6.Content);
        }

        [Fact]
        public async void GetAsyncWithPaginationTakeNegative()
        {
            // Arrange
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "CommentGetAsyncWithPaginationTakeNegative@email.com", Password = "1234", UserName = "ComGetAWithPagTakeNega" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "ComGetAWithAllArgs" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "ComGetAWithAllArgs", Content = "ComGetAWithAllArgs", Category = category.Entity });
            var testComment = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncWithPaginationTakeNegative1"
            };
            var testComment2 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncWithPaginationTakeNegative2"
            };
            var testComment3 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncWithPaginationTakeNegative3"
            };
            var testComment5 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncWithPaginationTakeNegative4"
            };
            var testComment6 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncWithPaginationTakeNegative5"
            };
            var testComment4 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncWithPaginationTakeNegative6"
            };
            await commentRepository.AddAsync(testComment);
            await commentRepository.AddAsync(testComment2);
            await commentRepository.AddAsync(testComment3);
            await commentRepository.AddAsync(testComment4);
            await commentRepository.AddAsync(testComment5);
            await commentRepository.AddAsync(testComment6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await commentRepository.GetAsync(new ContentContainsSpecification<Comment>("GetAsyncWithPaginationTakeNegative"),
                new PagingSpecification(2, -2),
                new SortSpecification<Comment>(
                    new OrderBySpecification<Comment>(x => x.Content),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(!result.Any());
        }

        [Fact]
        public async void GetAsyncWithPaginationSkipNegative()
        {
            // Arrange
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "CommentGetAsyncWithPaginationSkipNegative@email.com", Password = "1234", UserName = "ComGetAWithPagSkipNega" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "ComGetAWithPagSkipNega" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "ComGetAWithPagSkipNega", Content = "ComGetAWithPagSkipNega", Category = category.Entity });
            var testComment = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncWithPaginationSkipNegative"
            };
            var testComment2 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncWithPaginationSkipNegative2"
            };
            var testComment3 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncWithPaginationSkipNegative3"
            };
            var testComment4 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncWithPaginationSkipNegative4"
            };
            var testComment5 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncWithPaginationSkipSkipNegative5"
            };
            var testComment6 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncWithPaginationSkipSkipNegative6"
            };
            await commentRepository.AddAsync(testComment);
            await commentRepository.AddAsync(testComment2);
            await commentRepository.AddAsync(testComment3);
            await commentRepository.AddAsync(testComment4);
            await commentRepository.AddAsync(testComment5);
            await commentRepository.AddAsync(testComment6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await commentRepository.GetAsync(new ContentContainsSpecification<Comment>("GetAsyncWithPaginationSkipNegative"),
                new PagingSpecification(-2, 3),
                new SortSpecification<Comment>(
                    new OrderBySpecification<Comment>(x => x.Content),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count() == 3);
            Assert.Contains(result, x => x.Id == testComment.Id &&
                                         x.Content == testComment.Content);
            Assert.Contains(result, x => x.Id == testComment2.Id &&
                                         x.Content == testComment2.Content);
            Assert.Contains(result, x => x.Id == testComment3.Id &&
                                         x.Content == testComment3.Content);
        }

        [Fact]
        public async void GetAsyncWithPaginationSkipOutOfRange()
        {
            // Arrange
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "CommentGetAsyncWithPaginationSkipOutOfRange@email.com", Password = "1234", UserName = "ComGetAWiPagSkipOfRa" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "ComGetAWiPagSkipOfRa" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "ComGetAWiPagSkipOfRa", Content = "ComGetAWiPagSkipOfRa", Category = category.Entity });
            var testComment = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncWithPaginationSkipOutOfRange"
            };
            var testComment2 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncWithPaginationSkipOutOfRange2"
            };
            var testComment3 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncWithPaginationSkipOutOfRange3"
            };
            var testComment5 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncWithPaginationSkipOutOfRange4"
            };
            var testComment6 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncWithPaginationSkipOutOfRange5"
            };
            var testComment4 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentGetAsyncWithPaginationSkipOutOfRange6"
            };
            await commentRepository.AddAsync(testComment);
            await commentRepository.AddAsync(testComment2);
            await commentRepository.AddAsync(testComment3);
            await commentRepository.AddAsync(testComment4);
            await commentRepository.AddAsync(testComment5);
            await commentRepository.AddAsync(testComment6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await commentRepository.GetAsync(new ContentContainsSpecification<Comment>("GetAsyncWithPaginationSkipOutOfRange"),
                new PagingSpecification(7, 3),
                new SortSpecification<Comment>(
                    new OrderBySpecification<Comment>(x => x.Content),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(!result.Any());
        }

        [Fact]
        public void Remove()
        {
            // Arrange
            var nbCategoriesAtBeginning = _fixture.Db.Comments.Count();
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);
            var user = _fixture.Db.Users.Add(
                new User() { EmailAddress = "CommentRemove@email.com", Password = "1234", UserName = "CommentRemove" });
            var category = _fixture.Db.Categories.Add(new Category() { Name = "CCommentGetRemove" });
            var post = _fixture.Db.Posts.Add(
                new Post() { Author = user.Entity, Name = "CommentRemove", Content = "CommenRemove", Category = category.Entity });
            var testComment = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "Remove"
            };
            commentRepository.Add(testComment);
            _fixture.UnitOfWork.Save();
            var nbCategoriesAfterAdded = _fixture.Db.Comments.Count();

            // Act
            commentRepository.Remove(testComment);
            _fixture.UnitOfWork.Save();

            // Assert
            var nbCategoriesAfterRemoved = _fixture.Db.Comments.Count();
            Assert.True(nbCategoriesAtBeginning + 1 == nbCategoriesAfterAdded &&
                        nbCategoriesAfterRemoved == nbCategoriesAtBeginning);
        }

        [Fact]
        public void GetComment()
        {
            // Arrange
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);
            var user = _fixture.Db.Users.Add(
                new User() { EmailAddress = "GetComment@email.com", Password = "1234", UserName = "GetComment" });
            var category = _fixture.Db.Categories.Add(new Category() { Name = "GetComment" });
            var post = _fixture.Db.Posts.Add(
                new Post() { Author = user.Entity, Name = "GetComment", Content = "GetComment", Category = category.Entity });
            var testComment = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "GetComment"
            };
            commentRepository.Add(testComment);
            _fixture.UnitOfWork.Save();

            // Act
            var result = commentRepository.Get(testComment.Id);

            // Act & Assert
            Assert.True(result == _fixture.Db.Comments.Find(testComment.Id));
        }

        [Fact]
        public void GetCategoryOutOfRange()
        {
            // Arrange
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);

            // Act & Assert
            Assert.Throws<IndexOutOfRangeException>(() => commentRepository.Get(100));
        }

        [Fact]
        public void RemoveNull()
        {
            // Arrange
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => commentRepository.Remove(null));
        }

        [Fact]
        public async void RemoveRangeAsyncNull()
        {
            // Arrange
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await commentRepository.RemoveRangeAsync(null));
        }

        [Fact]
        public void RemoveRangeNull()
        {
            // Arrange
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => commentRepository.RemoveRange(null));
        }

        [Fact]
        public void RemoveRange()
        {
            // Arrange
            var nbCategoriesAtBeginning = _fixture.Db.Comments.Count();
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);
            var category = _fixture.Db.Categories.Add(new Category() { Name = "CommentRemoveRange" });
            var user = _fixture.Db.Users.Add(
                new User() { EmailAddress = "CommentRemoveRange@email.com", Password = "1234", UserName = "CommentGetRemoveRg" });
            var post = _fixture.Db.Posts.Add(
                new Post() { Author = user.Entity, Name = "CommentRemoveRange", Content = "CommentRemoveRange", Category = category.Entity });
            var testComment = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentRemoveRange1"
            };
            var testComment2 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentRemoveRange2"
            };
            commentRepository.Add(testComment);
            commentRepository.Add(testComment2);
            _fixture.UnitOfWork.Save();
            var nbCategoriesAfterAdded = _fixture.Db.Comments.Count();

            // Act
            commentRepository.RemoveRange(new List<Comment>() { testComment, testComment2 });
            _fixture.UnitOfWork.Save();

            // Assert
            var nbCategoriesAfterRemoved = _fixture.Db.Comments.Count();
            Assert.True(nbCategoriesAtBeginning + 2 == nbCategoriesAfterAdded &&
                        nbCategoriesAfterRemoved == nbCategoriesAtBeginning);
        }

        [Fact]
        public async void RemoveRangeAsync()
        {
            // Arrange
            var nbCategoriesAtBeginning = _fixture.Db.Comments.Count();
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);
            var category = _fixture.Db.Categories.Add(new Category() { Name = "CommentRemoveRange" });
            var user = _fixture.Db.Users.Add(
                new User() { EmailAddress = "ComRemoveRangeAsync@email.com", Password = "1234", UserName = "ComRemoveRgAsync" });
            var post = _fixture.Db.Posts.Add(
                new Post() { Author = user.Entity, Name = "ComRemoveRgAsync", Content = "ComRemoveRgAsync", Category = category.Entity });
            var testComment = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentRemoveRangeAsync1"
            };
            var testComment2 = new Comment()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "CommentRemoveRangeAsync2"
            };
            await commentRepository.AddAsync(testComment);
            await commentRepository.AddAsync(testComment2);
            _fixture.UnitOfWork.Save();
            var nbCategoriesAfterAdded = _fixture.Db.Comments.Count();

            // Act
            await commentRepository.RemoveRangeAsync(new List<Comment>() { testComment, testComment2 });
            _fixture.UnitOfWork.Save();

            // Assert
            var nbCategoriesAfterRemoved = _fixture.Db.Comments.Count();
            Assert.True(nbCategoriesAtBeginning + 2 == nbCategoriesAfterAdded &&
                        nbCategoriesAfterRemoved == nbCategoriesAtBeginning);
        }
    }
}
