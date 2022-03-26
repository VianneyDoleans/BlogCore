using System;
using System.Collections.Generic;
using System.Linq;
using DbAccess.Data.POCO;
using DbAccess.Specifications;
using DbAccess.Specifications.FilterSpecifications.Filters;
using DbAccess.Specifications.SortSpecification;
using DBAccess.Tests.Builders;
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
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var user = new UserBuilder(userRepository, _fixture.UnitOfWork).Build();
            var post = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).Build();
            var testComment = new Comment() {Author = user, PostParent = post, Content = "AddCommentAsync" };

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
            var testComment = new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).Build();

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
            var testComment = new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).Build();
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
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var user = new UserBuilder(userRepository, _fixture.UnitOfWork).Build();
            var post = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).Build();
            var testComment = new Comment() { Author = user, PostParent = post, Content = "AddComment" };

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
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).Build();
            var testComment2 = new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).Build();
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).Build(); 
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).Build();

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
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncABSpecification").Build();
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncAUSpecification2").Build();
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncAKSpecification3").Build();
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncAKSpecification32").Build();
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncAWSpecification4").Build();
            var testComment = new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncAKSpecification3164").Build();

            // Act
            var result = (await commentRepository.GetAsync(new ContentContainsSpecification<Comment>("AK") & new ContentContainsSpecification<Comment>("164"))).ToList();

            // Assert
            Assert.True(result.Count() == 1 && result.First().Content == testComment.Content);
        }

        [Fact]
        public async void GetAsyncWithTwoSortsAndTwoSpecificationsAndPagination()
        {
            // Arrange
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            var user = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("TwoSortsAndTwoSpecCo").Build();
            var user2 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("TwoSortsAndTwoSpecCo2").Build();
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("TwwooGetAsyncWithTwoSortsComment").WithAuthor(user).Build();
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("GetAsyncWithTwoSorts2Comment").WithAuthor(user2).Build();
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("GetAsyncWithTwoSorts3TwwooComment").WithAuthor(user).Build();
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("GetAsyncTwwooWithTwoSorts5Comment").WithAuthor(user2).Build();
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("GetAsyncWithTwoSorts6Comment").WithAuthor(user).Build();
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("TwwooGetAsyncWithTwoorts7Comment").WithAuthor(user).Build();
            var comment = new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("AGetTwwooAsyncWithTwoSorts4Comment").WithAuthor(user2).Build();

            // Act
            var result = (await commentRepository.GetAsync(new ContentContainsSpecification<Comment>("WithTwoSorts")
                                                            & new ContentContainsSpecification<Comment>("Twwoo"), new PagingSpecification(0, 20),
                new SortSpecification<Comment>(new OrderBySpecification<Comment>(x => x.Author.UserName), SortingDirectionSpecification.Descending) &
                new SortSpecification<Comment>(new OrderBySpecification<Comment>(x => x.Content), SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count() == 4 && result.First().Content == comment.Content);
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
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncABSpecificationWithAllArguments").Build();
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncAUSpecification2WithAllArguments").Build();
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncAKSpecification3WithAllArguments").Build();
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncAKSpecification3WithAllArguments163").Build();
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncAWSpecification4WithAllArguments").Build();
            var testComment = new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncAKSpecification3WithAllArguments2").Build();

            // Act
            var result = (await commentRepository.GetAsync(new ContentContainsSpecification<Comment>("AK") &
                new ContentContainsSpecification<Comment>("WithAllArguments"),
                new PagingSpecification(0, 2),
                new SortSpecification<Comment>(
                    new OrderBySpecification<Comment>(x => x.Content),
                    SortingDirectionSpecification.Descending))).ToList();

            // Assert
            Assert.True(result.Count() == 2 && result.First().Content == testComment.Content);
        }

        [Fact]
        public async void GetAsyncWithPagination()
        {
            // Arrange
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("Comment1GetAsyncWithPagination1").Build();
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("Comment1GetAsyncWithPagination2").Build();
            var testComment3 = new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("Comment1GetAsyncWithPagination3").Build();
            var testComment4 = new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("Comment1GetAsyncWithPagination4").Build();
            var testComment5 = new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("Comment1GetAsyncWithPagination5").Build();
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("Comment1GetAsyncWithPagination6").Build();

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
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncWithPaginationTakeOutOfRange1").Build();
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncWithPaginationTakeOutOfRange2").Build();
            var testComment3 = new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncWithPaginationTakeOutOfRange3").Build();
            var testComment5 = new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncWithPaginationTakeOutOfRange4").Build();
            var testComment6 = new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncWithPaginationTakeOutOfRange5").Build();
            var testComment4 = new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncWithPaginationTakeOutOfRange6").Build();

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
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncWithPaginationTakeNegative1").Build();
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncWithPaginationTakeNegative2").Build();
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncWithPaginationTakeNegative3").Build();
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncWithPaginationTakeNegative4").Build();
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncWithPaginationTakeNegative5").Build();
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncWithPaginationTakeNegative6").Build();

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
            var testComment = new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncWithPaginationSkipNegative").Build();
            var testComment2 = new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncWithPaginationSkipNegative2").Build();
            var testComment3 = new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncWithPaginationSkipNegative3").Build();
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncWithPaginationSkipNegative4").Build();
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncWithPaginationSkipSkipNegative5").Build();
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncWithPaginationSkipSkipNegative6").Build();

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
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncWithPaginationSkipOutOfRange").Build();
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncWithPaginationSkipOutOfRange2").Build();
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncWithPaginationSkipOutOfRange3").Build();
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncWithPaginationSkipOutOfRange4").Build();
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncWithPaginationSkipOutOfRange5").Build();
            new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("CommentGetAsyncWithPaginationSkipOutOfRange6").Build();

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
            var testComment = new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).Build();
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
            var testComment = new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).Build();

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
            var testComment = new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).Build();
            var testComment2 = new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).Build();
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
            var testComment = new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).Build();
            var testComment2 = new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).Build();
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
