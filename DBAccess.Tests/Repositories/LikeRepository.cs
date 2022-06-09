using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbAccess.Data.POCO;
using DbAccess.Specifications;
using DbAccess.Specifications.FilterSpecifications.Filters;
using DbAccess.Specifications.SortSpecification;
using DBAccess.Tests.Builders;
using Xunit;

namespace DBAccess.Tests.Repositories
{
    public class LikesRepository
    {
        private readonly DatabaseFixture _fixture;

        public LikesRepository()
        {
            _fixture = new DatabaseFixture();
        }

        [Fact]
        public async Task AddLikesAsync()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var user = new UserBuilder(userRepository, _fixture.UnitOfWork).Build();
            var post = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).Build();
            var testLike = new Like()
            {
                User = user,
                LikeableType = LikeableType.Post,
                Post = post
            };

            // Act
            await likeRepository.AddAsync(testLike);
            _fixture.UnitOfWork.Save();

            // Assert
            Assert.True(_fixture.Db.Likes.First(x => x.Post == testLike.Post) != null);
        }

        [Fact]
        public async Task AddNullLikesAsync()
        {
            // Arrange
            var repository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.AddAsync(null));
        }

        [Fact]
        public async Task GetLikesAsync()
        {
            // Arrange
            var repository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);
            var like = new LikeBuilder(repository, _fixture.UnitOfWork, _fixture.Db).Build();

            // Act
            var result = await repository.GetAsync(like.Id);

            // Assert
            Assert.True(result == await _fixture.Db.Likes.FindAsync(like.Id));
        }

        [Fact]
        public async Task GetLikesOutOfRangeAsync()
        {
            // Arrange
            var repository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);

            // Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await repository.GetAsync(100));
        }

        [Fact]
        public async Task GetAllAsync()
        {
            // Arrange
            var repository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);
            var result = await repository.GetAllAsync();

            // Act & Assert
            Assert.True(result.Count() == _fixture.Db.Likes.Count());
        }

        [Fact]
        public async Task RemoveAsync()
        {
            // Arrange
            var nbLikesAtBeginning = _fixture.Db.Likes.Count();
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);
            var testLikes = new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).Build();
            var nbLikesAfterAdded = _fixture.Db.Likes.Count();
            
            // Act
            await likeRepository.RemoveAsync(testLikes);
            _fixture.UnitOfWork.Save();

            // Assert
            var nbLikesAfterRemoved = _fixture.Db.Likes.Count();
            Assert.True(nbLikesAtBeginning + 1 == nbLikesAfterAdded &&
                        nbLikesAfterRemoved == nbLikesAtBeginning);
        }

        [Fact]
        public async Task RemoveNullAsync()
        {
            // Arrange
            var repository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.RemoveAsync(null));
        }

        // -------

        [Fact]
        public void AddLike()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var user = new UserBuilder(userRepository, _fixture.UnitOfWork).Build();
            var post = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).Build();
            var testLike = new Like()
            {
                User = user,
                LikeableType = LikeableType.Post,
                Post = post
            };

            // Act
            var like = likeRepository.Add(testLike);
            _fixture.UnitOfWork.Save();

            // Assert
            Assert.True(_fixture.Db.Likes.First(x => x.Id == like.Id) != null);
        }

        [Fact]
        public void AddNullLike()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                likeRepository.Add(null);
                _fixture.Db.SaveChanges();
            });
        }

        [Fact]
        public void CountAll()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);

            // Act & Assert
            Assert.True(_fixture.Db.Likes.Count() == likeRepository.CountAll());
        }

        [Fact]
        public async Task CountAllAsync()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);

            // Act & Assert
            Assert.True(_fixture.Db.Likes.Count() == await likeRepository.CountAllAsync());
        }


        [Fact]
        public void GetAll()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);

            // Act
            var result = likeRepository.GetAll();

            // Assert
            Assert.True(result.Count() == _fixture.Db.Likes.Count());
        }

        [Fact]
        public async Task GetAsyncSpecificationBasic()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).Build();
            var testLike = new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).Build();
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).Build();
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).Build();

            // Act
            var result = await likeRepository.GetAsync(new IdSpecification<Like>(testLike.Id));

            // Assert
            Assert.True(result.First().Id == testLike.Id);
        }

        [Fact]
        public async Task GetAsyncWithTwoSpecifications()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            var user = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("GetAWiTwoSpecLi").Build();
            var user2 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("GetAWiTwoSpecLi2").Build();
            var user3 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("GetAWiTwoSpecLi3").Build();
            var user4 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("GetAWiTwoSpecLi4").Build();
            var user5 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("GetAWiTwoSpecLi5").Build();
            var user6 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("GetAWiTwoSpecLi6").Build();
            new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("GetAWiTwoSpecLi7").Build();
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user).Build();
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user2).Build();
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user3).Build();
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user5).Build();
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user6).Build();
            var testLike = new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user4).Build();

            // Act
            var result = (await likeRepository.GetAsync(new UserUsernameContainsSpecification<Like>("GetAWiTwoSpecLi") & new UserUsernameContainsSpecification<Like>("4"))).ToList();

            // Assert
            Assert.True(result.Count == 1 && result.First().User.Id == testLike.User.Id);
        }

        [Fact]
        public async Task GetAsyncWithTwoSortsAndTwoSpecificationsAndPagination()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);

            var user = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiToSosAdTwoSpcWiPa").Build();
            var user2 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiToSosAdTwoSpcWiPa2").Build();
            var user3 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiToSosAdTwoSpcWiPa3").Build();
            var user4 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiToSosAdTwoSpcWiPa4").Build();
            var user5 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiToSosAdTwoSpcWiPa5").Build();
            var user6 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("ALiToSosAdTwoSpcWiPe6").Build();
            
            var post = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithName("LikeTwoSortsAndTwoSpecApa").WithAuthor(user).Build();
            var comment = new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithAuthor(user).Build();
            
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user).WithPost(post).Build();
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user).WithComment(comment).Build();
            var like3 = new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user2).WithPost(post).Build();
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user3).WithComment(comment).Build();
            var like5 = new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user4).WithPost(post).Build();
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user5).WithPost(post).Build();
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user6).WithComment(comment).Build();

            // Act
            var result = (await likeRepository.GetAsync(new UserUsernameContainsSpecification<Like>("SpcWiPa")
                                                            & new PostNameSpecification<Like>("LikeTwoSortsAndTwoSpecApa"), new PagingSpecification(1, 2),
                new SortSpecification<Like>(new OrderBySpecification<Like>(x => x.User.UserName), SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count == 2);
            Assert.Contains(result, x => x.Id == like3.Id);
            Assert.Contains(result, x => x.Id == like5.Id);
        }

        [Fact]
        public async Task GetAsyncWithNoArgument()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);

            // Act & Assert
            Assert.True((await likeRepository.GetAsync()).ToList().Count == _fixture.Db.Likes.Count());
        }

        [Fact]
        public async Task GetAsyncWithAllArguments()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);

            var user = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiAllArg").WithEmailAddress("BELiGetAWiAllArg@email.com").Build();
            var user2 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiAllArg2").WithEmailAddress("AELiGetAWiAllArg2@email.com").Build();
            var user3 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiAllArg3").WithEmailAddress("ELiGetAWiAllArg3@email.com").Build();
            var user4 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiAllArg4").WithEmailAddress("GLiGetAWiAllArg4@email.com").Build();
            var user5 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiAllArg5").WithEmailAddress("ELiGetAWiAllArg5@email.com").Build();
            var user6 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiAllArg6").WithEmailAddress("ALiGetAWiAllArg6@email.com").Build();
            var post = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).Build();
            var comment = new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).Build();

            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user).WithPost(post).Build();
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user).WithComment(comment).Build();
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user2).WithPost(post).Build();
            var like4 = new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user3).WithComment(comment).Build();
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user4).WithPost(post).Build();
            var like6 = new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user5).WithPost(post).Build();
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user6).WithComment(comment).Build();

            // Act
            var result = (await likeRepository.GetAsync(new UserUsernameContainsSpecification<Like>("LiGet") &
                                                        new UserUsernameContainsSpecification<Like>("AllArg"),
                new PagingSpecification(1, 2),
                new SortSpecification<Like>(
                    new OrderBySpecification<Like>(x => x.User.Email),
                    SortingDirectionSpecification.Descending))).ToList();

            // Assert
            Assert.True(result.Count == 2 && result.First().User.Email == "ELiGetAWiAllArg5@email.com");
            Assert.Contains(result, x => x.Id == like6.Id);
            Assert.Contains(result, x => x.Id == like4.Id);
        }

        [Fact]
        public async Task GetAsyncWithPagination()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db); 
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);

            var user = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiPa").Build();
            var user2 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiPa2").Build();
            var user3 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiPa3").Build();
            var user4 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiPa4").Build();
            var user5 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiPa5").Build();
            var user6 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiPa6").Build();

            var post = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).Build();
            var comment = new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).Build();


            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user).WithPost(post).Build();
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user).WithComment(comment).Build();
            var like3 = new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user2).WithPost(post).Build();
            var like4 = new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user3).WithComment(comment).Build();
            var like5 = new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user4).WithPost(post).Build();
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user5).WithPost(post).Build();
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user6).WithComment(comment).Build();

            // Act
            var result = (await likeRepository.GetAsync(new UserUsernameContainsSpecification<Like>("LiGetAWiPa"),
                new PagingSpecification(2, 3),
                new SortSpecification<Like>(
                    new OrderBySpecification<Like>(x => x.User.UserName),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count == 3);
            Assert.Contains(result, x => x.Id == like3.Id &&
                                         x.User.UserName == like3.User.UserName);
            Assert.Contains(result, x => x.Id == like4.Id &&
                                          x.User.UserName == like4.User.UserName);
            Assert.Contains(result, x => x.Id == like5.Id &&
                                         x.User.UserName == like5.User.UserName);
        }

        [Fact]
        public async Task GetAsyncWithPaginationTakeOutOfRange()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);

            var user = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiPaTafRa").Build();
            var user2 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiPaTafRa2").Build();
            var user3 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiPaTafRa3").Build();
            var user4 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiPaTafRa4").Build();
            var user5 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiPaTafRa5").Build();
            var user6 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiPaTafRa6").Build();

            var post = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).Build();
            var comment = new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).Build();

            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user).WithPost(post).Build();
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user).WithComment(comment).Build();
            var like3 = new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user2).WithPost(post).Build();
            var like4 = new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user3).WithComment(comment).Build();
            var like5 = new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user4).WithPost(post).Build();
            var like6 = new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user5).WithPost(post).Build();
            var like7 = new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user6).WithComment(comment).Build();

            // Act
            var result = (await likeRepository.GetAsync(new UserUsernameContainsSpecification<Like>("LiGetAWiPaTafRa"),
                new PagingSpecification(2, 22),
                new SortSpecification<Like>(
                    new OrderBySpecification<Like>(x => x.User.UserName),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count == 5);
            Assert.Contains(result, x => x.Id == like3.Id &&
                                         x.User.UserName == like3.User.UserName);
            Assert.Contains(result, x => x.Id == like4.Id &&
                                         x.User.UserName == like4.User.UserName);
            Assert.Contains(result, x => x.Id == like5.Id &&
                                         x.User.UserName == like5.User.UserName);
            Assert.Contains(result, x => x.Id == like6.Id &&
                                         x.User.UserName == like6.User.UserName);
            Assert.Contains(result, x => x.Id == like7.Id &&
                                         x.User.UserName == like7.User.UserName);
        }

        [Fact]
        public async Task GetAsyncWithPaginationTakeNegative()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);

            var user = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiPaTaNe").Build();
            var user2 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiPaTaNe2").Build();
            var user3 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiPaTaNe3").Build();
            var user4 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiPaTaNe4").Build();
            var user5 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiPaTaNe5").Build();
            var user6 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiPaTaNe6").Build();

            var post = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).Build();
            var comment = new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).Build();

            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user).WithPost(post).Build();
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user).WithComment(comment).Build();
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user2).WithPost(post).Build();
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user3).WithComment(comment).Build();
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user4).WithPost(post).Build();
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user5).WithPost(post).Build();
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user6).WithComment(comment).Build();

            // Act
            var result = (await likeRepository.GetAsync(new UserUsernameContainsSpecification<Like>("LiGetAWiPaTaNe"),
                new PagingSpecification(2, -2),
                new SortSpecification<Like>(
                    new OrderBySpecification<Like>(x => x.User.UserName),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(!result.Any());
        }

        [Fact]
        public async Task GetAsyncWithPaginationSkipNegative()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);

            var user = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiPaSkiN").Build();
            var user2 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiPaSkiN2").Build();
            var user3 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiPaSkiN3").Build();
            var user4 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiPaSkiN4").Build();
            var user5 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiPaSkiN5").Build();
            var user6 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiPaSkiN6").Build();

            var post = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).Build();
            var comment = new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).Build();

            var like = new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user).WithPost(post).Build();
            var like2 = new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user).WithComment(comment).Build();
            var like3 = new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user2).WithPost(post).Build();
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user3).WithComment(comment).Build();
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user4).WithPost(post).Build();
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user5).WithPost(post).Build();
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user6).WithComment(comment).Build();

            // Act
            var result = (await likeRepository.GetAsync(new UserUsernameContainsSpecification<Like>("LiGetAWiPaSkiN"),
                new PagingSpecification(-2, 3),
                new SortSpecification<Like>(
                    new OrderBySpecification<Like>(x => x.User.UserName),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count == 3);
            Assert.Contains(result, x => x.Id == like.Id &&
                                         x.User.UserName == like.User.UserName && like.LikeableType == LikeableType.Post);
            Assert.Contains(result, x => x.Id == like2.Id &&
                                         x.User.UserName == like2.User.UserName && like2.LikeableType == LikeableType.Comment);
            Assert.Contains(result, x => x.Id == like3.Id &&
                                         x.User.UserName == like3.User.UserName);
        }

        [Fact]
        public async Task GetAsyncWithPaginationSkipOutOfRange()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var commentRepository = new DbAccess.Repositories.Comment.CommentRepository(_fixture.Db);

            var user = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiPaSkORaN").Build();
            var user2 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiPaSkORa2").Build();
            var user3 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiPaSkORa3").Build();
            var user4 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiPaSkORa4").Build();
            var user5 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiPaSkORa5").Build();
            var user6 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("LiGetAWiPaSkORa6").Build();
            var post = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).Build();
            var comment = new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).Build();
            
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user).WithPost(post).Build();
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user).WithComment(comment).Build();
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user2).WithPost(post).Build();
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user3).WithComment(comment).Build();
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user4).WithPost(post).Build();
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user5).WithPost(post).Build();
            new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).WithUser(user6).WithComment(comment).Build();


            // Act
            var result = (await likeRepository.GetAsync(new UserUsernameContainsSpecification<Like>("LiGetAWiPaSkORa"),
                new PagingSpecification(7, 3),
                new SortSpecification<Like>(
                    new OrderBySpecification<Like>(x => x.User.UserName),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(!result.Any());
        }

        [Fact]
        public void Remove()
        {
            // Arrange
            var nbLikesAtBeginning = _fixture.Db.Likes.Count();
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);
            var like = new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).Build();
            var nbLikesAfterAdded = _fixture.Db.Likes.Count();

            // Act
            likeRepository.Remove(like);
            _fixture.UnitOfWork.Save();

            // Assert
            var nbLikesAfterRemoved = _fixture.Db.Likes.Count();
            Assert.True(nbLikesAtBeginning + 1 == nbLikesAfterAdded &&
                        nbLikesAfterRemoved == nbLikesAtBeginning);
        }

        [Fact]
        public void GetLike()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);
            var like = new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).Build();

            // Act
            var result = likeRepository.Get(like.Id);

            // Act & Assert
            Assert.True(result == _fixture.Db.Likes.Find(like.Id));
        }

        [Fact]
        public void GetCategoryOutOfRange()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);

            // Act & Assert
            Assert.Throws<IndexOutOfRangeException>(() => likeRepository.Get(100));
        }

        [Fact]
        public void RemoveNull()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => likeRepository.Remove(null));
        }

        [Fact]
        public async Task RemoveRangeAsyncNull()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await likeRepository.RemoveRangeAsync(null));
        }

        [Fact]
        public void RemoveRangeNull()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => likeRepository.RemoveRange(null));
        }

        [Fact]
        public void RemoveRange()
        {
            // Arrange
            var nbLikesAtBeginning = _fixture.Db.Likes.Count();
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);

            var like = new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).Build();
            var like2 = new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).Build();
            var nbLikesAfterAdded = _fixture.Db.Likes.Count();

            // Act
            likeRepository.RemoveRange(new List<Like>() { like, like2});
            _fixture.UnitOfWork.Save();

            // Assert
            var nbLikesAfterRemoved = _fixture.Db.Likes.Count();
            Assert.True(nbLikesAtBeginning + 2 == nbLikesAfterAdded &&
                        nbLikesAfterRemoved == nbLikesAtBeginning);
        }

        [Fact]
        public async Task RemoveRangeAsync()
        {
            // Arrange
            var nbLikesAtBeginning = _fixture.Db.Likes.Count();
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);
            var like = new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).Build();
            var like2 = new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).Build();
            var nbLikesAfterAdded = _fixture.Db.Likes.Count();

            // Act
            await likeRepository.RemoveRangeAsync(new List<Like>() { like, like2 });
            _fixture.UnitOfWork.Save();

            // Assert
            var nbLikesAfterRemoved = _fixture.Db.Likes.Count();
            Assert.True(nbLikesAtBeginning + 2 == nbLikesAfterAdded &&
                        nbLikesAfterRemoved == nbLikesAtBeginning);
        }

        [Fact]
        public async Task LikeAlreadyExistsFalse()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var user = new UserBuilder(userRepository, _fixture.UnitOfWork).Build();
            var post = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).Build();
            var testLike = new Like()
            {
                User = user,
                LikeableType = LikeableType.Post,
                Post = post
            };

            // Act & Assert
            Assert.True(!await likeRepository.LikeAlreadyExists(testLike));
        }

        [Fact]
        public async Task LikeAlreadyExistsNull()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);

            // Act & Assert
            Assert.True(!await likeRepository.LikeAlreadyExists(null));
        }

        [Fact]
        public async Task LikeAlreadyExistsTrue()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);

            var testLike = new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).Build();

            // Act & Assert
            Assert.True(await likeRepository.LikeAlreadyExists(testLike));
        }
    }
}
