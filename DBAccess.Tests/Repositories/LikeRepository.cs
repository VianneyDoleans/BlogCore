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
    public class LikesRepository
    {
        private readonly DatabaseFixture _fixture;

        public LikesRepository()
        {
            _fixture = new DatabaseFixture();
        }

        [Fact]
        public async void AddLikesAsync()
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
        public async void AddNullLikesAsync()
        {
            // Arrange
            var repository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.AddAsync(null));
        }

        [Fact]
        public async void GetLikesAsync()
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
        public async void GetLikesOutOfRangeAsync()
        {
            // Arrange
            var repository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);

            // Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await repository.GetAsync(100));
        }

        [Fact]
        public async void GetAllAsync()
        {
            // Arrange
            var repository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);
            var result = await repository.GetAllAsync();

            // Act & Assert
            Assert.True(result.Count() == _fixture.Db.Likes.Count());
        }

        [Fact]
        public async void RemoveAsync()
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
        public async void RemoveNullAsync()
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
        public async void CountAllAsync()
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
        public async void GetAsyncSpecificationBasic()
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
        public async void GetAsyncWithTwoSpecifications()
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
            Assert.True(result.Count() == 1 && result.First().User.Id == testLike.User.Id);
        }

        [Fact]
        public async void GetAsyncWithTwoSortsAndTwoSpecificationsAndPagination()
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
            Assert.True(result.Count() == 2);
            Assert.Contains(result, x => x.Id == like3.Id);
            Assert.Contains(result, x => x.Id == like5.Id);
        }

        [Fact]
        public async void GetAsyncWithNoArgument()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);

            // Act & Assert
            Assert.True((await likeRepository.GetAsync()).ToList().Count() == _fixture.Db.Likes.Count());
        }

        [Fact]
        public async void GetAsyncWithAllArguments()
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
            var post = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithName("LikeTwoSortsAndTwoSpecApa").WithAuthor(user).Build();
            var comment = new CommentBuilder(commentRepository, _fixture.UnitOfWork, _fixture.Db).WithAuthor(user).WithContent("LiGetAWiAllArgCo").Build();

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
                    new OrderBySpecification<Like>(x => x.User.EmailAddress),
                    SortingDirectionSpecification.Descending))).ToList();

            // Assert
            Assert.True(result.Count() == 2 && result.First().User.EmailAddress == "ELiGetAWiAllArg5@email.com");
            Assert.Contains(result, x => x.Id == like6.Id);
            Assert.Contains(result, x => x.Id == like4.Id);
        }

        [Fact]
        public async void GetAsyncWithPagination()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);

            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "ALiGetAWiPa@email.com", Password = "1234", UserName = "LiGetAWiPa" });
            var user2 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "ALiGetAWiPa2@email.com", Password = "1234", UserName = "LiGetAWiPa2" });
            var user3 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiGetAWiPa3@email.com", Password = "1234", UserName = "LiGetAWiPa3" });
            var user4 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "ALiGetAWiPa4@email.com", Password = "1234", UserName = "LiGetAWiPa4" });
            var user5 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiGetAWiPa5@email.com", Password = "1234", UserName = "LiGetAWiPa5" });
            var user6 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiGetAWiPa6@email.com", Password = "1234", UserName = "LiGetAWiPa6" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "LiGetAWiPaCa" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "LiGetAWiPaPo", Content = "LiGetAWiPaPo", Category = category.Entity });
            var comment = await _fixture.Db.Comments.AddAsync(new Comment()
            {
                Author = user.Entity,
                Content = "LiGetAWiPaCo",
                PostParent = post.Entity
            });
            await _fixture.Db.Likes.AddAsync(
                new Like() { User = user.Entity, Post = post.Entity, LikeableType = LikeableType.Post });
            await _fixture.Db.Likes.AddAsync(
                new Like() { User = user.Entity, Comment = comment.Entity, LikeableType = LikeableType.Comment });
            var like3 = await _fixture.Db.Likes.AddAsync(
                new Like() { User = user2.Entity, Post = post.Entity, LikeableType = LikeableType.Post });
            var like4 = await _fixture.Db.Likes.AddAsync(
                new Like() { User = user3.Entity, Comment = comment.Entity, LikeableType = LikeableType.Comment });
            var like5 = await _fixture.Db.Likes.AddAsync(
                new Like() { User = user4.Entity, Post = post.Entity, LikeableType = LikeableType.Post });
            await _fixture.Db.Likes.AddAsync(
                new Like() { User = user5.Entity, Post = post.Entity, LikeableType = LikeableType.Post });
            await _fixture.Db.Likes.AddAsync(
                new Like() { User = user6.Entity, Comment = comment.Entity, LikeableType = LikeableType.Comment });
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await likeRepository.GetAsync(new UserUsernameContainsSpecification<Like>("LiGetAWiPa"),
                new PagingSpecification(2, 3),
                new SortSpecification<Like>(
                    new OrderBySpecification<Like>(x => x.User.UserName),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count() == 3);
            Assert.Contains(result, x => x.Id == like3.Entity.Id &&
                                         x.User.UserName == like3.Entity.User.UserName);
            Assert.Contains(result, x => x.Id == like4.Entity.Id &&
                                          x.User.UserName == like4.Entity.User.UserName);
            Assert.Contains(result, x => x.Id == like5.Entity.Id &&
                                         x.User.UserName == like5.Entity.User.UserName);
        }

        [Fact]
        public async void GetAsyncWithPaginationTakeOutOfRange()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);

            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiGetAWiPaTafRa@email.com", Password = "1234", UserName = "LiGetAWiPaTafRa" });
            var user2 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiGetAWiPaTafRa2@email.com", Password = "1234", UserName = "LiGetAWiPaTafRa2" });
            var user3 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiGetAWiPaTafRa3@email.com", Password = "1234", UserName = "LiGetAWiPaTafRa3" });
            var user4 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiGetAWiPaTafRa4@email.com", Password = "1234", UserName = "LiGetAWiPaTafRa4" });
            var user5 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiGetAWiPaTafRa5@email.com", Password = "1234", UserName = "LiGetAWiPaTafRa5" });
            var user6 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiGetAWiPaTafRa6@email.com", Password = "1234", UserName = "LiGetAWiPaTafRa6" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "LiGetAWiPaTafRaCa" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "LiGetAWiPaTafRaPo", Content = "LiGetAWiPaTafRaPo", Category = category.Entity });
            var comment = await _fixture.Db.Comments.AddAsync(new Comment()
            {
                Author = user.Entity,
                Content = "LiGetAWiPaTafRaCo",
                PostParent = post.Entity
            });
            await _fixture.Db.Likes.AddAsync(
                new Like() { User = user.Entity, Post = post.Entity, LikeableType = LikeableType.Post });
            await _fixture.Db.Likes.AddAsync(
                new Like() { User = user.Entity, Comment = comment.Entity, LikeableType = LikeableType.Comment });
            var like3 = await _fixture.Db.Likes.AddAsync(
                new Like() { User = user2.Entity, Post = post.Entity, LikeableType = LikeableType.Post });
            var like4 = await _fixture.Db.Likes.AddAsync(
                new Like() { User = user3.Entity, Comment = comment.Entity, LikeableType = LikeableType.Comment });
            var like5 = await _fixture.Db.Likes.AddAsync(
                new Like() { User = user4.Entity, Post = post.Entity, LikeableType = LikeableType.Post });
            var like6 = await _fixture.Db.Likes.AddAsync(
                new Like() { User = user5.Entity, Post = post.Entity, LikeableType = LikeableType.Post });
            var like7 = await _fixture.Db.Likes.AddAsync(
                new Like() { User = user6.Entity, Comment = comment.Entity, LikeableType = LikeableType.Comment });
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await likeRepository.GetAsync(new UserUsernameContainsSpecification<Like>("LiGetAWiPaTafRa"),
                new PagingSpecification(2, 22),
                new SortSpecification<Like>(
                    new OrderBySpecification<Like>(x => x.User.UserName),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count() == 5);
            Assert.Contains(result, x => x.Id == like3.Entity.Id &&
                                         x.User.UserName == like3.Entity.User.UserName);
            Assert.Contains(result, x => x.Id == like4.Entity.Id &&
                                         x.User.UserName == like4.Entity.User.UserName);
            Assert.Contains(result, x => x.Id == like5.Entity.Id &&
                                         x.User.UserName == like5.Entity.User.UserName);
            Assert.Contains(result, x => x.Id == like6.Entity.Id &&
                                         x.User.UserName == like6.Entity.User.UserName);
            Assert.Contains(result, x => x.Id == like7.Entity.Id &&
                                         x.User.UserName == like7.Entity.User.UserName);
        }

        [Fact]
        public async void GetAsyncWithPaginationTakeNegative()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);

            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiGetAWiPaTaNe@email.com", Password = "1234", UserName = "LiGetAWiPaTaNe" });
            var user2 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiGetAWiPaTaNe2@email.com", Password = "1234", UserName = "LiGetAWiPaTaNe2" });
            var user3 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiGetAWiPaTaNe3@email.com", Password = "1234", UserName = "LiGetAWiPaTaNe3" });
            var user4 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiGetAWiPaTaNe4@email.com", Password = "1234", UserName = "LiGetAWiPaTaNe4" });
            var user5 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiGetAWiPaTaNe5@email.com", Password = "1234", UserName = "LiGetAWiPaTaNe5" });
            var user6 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiGetAWiPaTaNe6@email.com", Password = "1234", UserName = "LiGetAWiPaTaNe6" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "LiGetAWiPaTaNeCa" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "LiGetAWiPaTaNePo", Content = "LiGetAWiPaTaNePo", Category = category.Entity });
            var comment = await _fixture.Db.Comments.AddAsync(new Comment()
            {
                Author = user.Entity,
                Content = "LiGetAWiPaTafRaCo",
                PostParent = post.Entity
            });
            await _fixture.Db.Likes.AddAsync(
                new Like() { User = user.Entity, Post = post.Entity, LikeableType = LikeableType.Post });
            await _fixture.Db.Likes.AddAsync(
                new Like() { User = user.Entity, Comment = comment.Entity, LikeableType = LikeableType.Comment });
            await _fixture.Db.Likes.AddAsync(
                new Like() { User = user2.Entity, Post = post.Entity, LikeableType = LikeableType.Post });
            await _fixture.Db.Likes.AddAsync(
                new Like() { User = user3.Entity, Comment = comment.Entity, LikeableType = LikeableType.Comment });
            await _fixture.Db.Likes.AddAsync(
                new Like() { User = user4.Entity, Post = post.Entity, LikeableType = LikeableType.Post });
            await _fixture.Db.Likes.AddAsync(
                new Like() { User = user5.Entity, Post = post.Entity, LikeableType = LikeableType.Post });
            await _fixture.Db.Likes.AddAsync(
                new Like() { User = user6.Entity, Comment = comment.Entity, LikeableType = LikeableType.Comment });
            _fixture.UnitOfWork.Save();

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
        public async void GetAsyncWithPaginationSkipNegative()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);

            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiGetAWiPaSkiN@email.com", Password = "1234", UserName = "LiGetAWiPaSkiN" });
            var user2 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiGetAWiPaSkiN2@email.com", Password = "1234", UserName = "LiGetAWiPaSkiN2" });
            var user3 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiGetAWiPaSkiN3@email.com", Password = "1234", UserName = "LiGetAWiPaSkiN3" });
            var user4 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiGetAWiPaSkiN4@email.com", Password = "1234", UserName = "LiGetAWiPaSkiN4" });
            var user5 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiGetAWiPaSkiN5@email.com", Password = "1234", UserName = "LiGetAWiPaSkiN5" });
            var user6 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiGetAWiPaSkiN6@email.com", Password = "1234", UserName = "LiGetAWiPaSkiN6" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "LiGetAWiPaSkiNCa" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "LiGetAWiPaSkiNPo", Content = "LiGetAWiPaSkiNPo", Category = category.Entity });
            var comment = await _fixture.Db.Comments.AddAsync(new Comment()
            {
                Author = user.Entity,
                Content = "LiGetAWiPaSkiNCo",
                PostParent = post.Entity
            });
            var like = await _fixture.Db.Likes.AddAsync(
                new Like() { User = user.Entity, Post = post.Entity, LikeableType = LikeableType.Post });
            var like2 = await _fixture.Db.Likes.AddAsync(
                new Like() { User = user.Entity, Comment = comment.Entity, LikeableType = LikeableType.Comment });
            var like3 = await _fixture.Db.Likes.AddAsync(
                new Like() { User = user2.Entity, Post = post.Entity, LikeableType = LikeableType.Post });
            await _fixture.Db.Likes.AddAsync(
                new Like() { User = user3.Entity, Comment = comment.Entity, LikeableType = LikeableType.Comment });
            await _fixture.Db.Likes.AddAsync(
                new Like() { User = user4.Entity, Post = post.Entity, LikeableType = LikeableType.Post });
            await _fixture.Db.Likes.AddAsync(
                new Like() { User = user5.Entity, Post = post.Entity, LikeableType = LikeableType.Post });
            await _fixture.Db.Likes.AddAsync(
                new Like() { User = user6.Entity, Comment = comment.Entity, LikeableType = LikeableType.Comment });
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await likeRepository.GetAsync(new UserUsernameContainsSpecification<Like>("LiGetAWiPaSkiN"),
                new PagingSpecification(-2, 3),
                new SortSpecification<Like>(
                    new OrderBySpecification<Like>(x => x.User.UserName),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count() == 3);
            Assert.Contains(result, x => x.Id == like.Entity.Id &&
                                         x.User.UserName == like.Entity.User.UserName && like.Entity.LikeableType == LikeableType.Post);
            Assert.Contains(result, x => x.Id == like2.Entity.Id &&
                                         x.User.UserName == like2.Entity.User.UserName && like2.Entity.LikeableType == LikeableType.Comment);
            Assert.Contains(result, x => x.Id == like3.Entity.Id &&
                                         x.User.UserName == like3.Entity.User.UserName);
        }

        [Fact]
        public async void GetAsyncWithPaginationSkipOutOfRange()
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
        public async void RemoveRangeAsyncNull()
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
        public async void RemoveRangeAsync()
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
        public async void LikeAlreadyExistsFalse()
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
        public async void LikeAlreadyExistsNull()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);

            // Act & Assert
            Assert.True(!await likeRepository.LikeAlreadyExists(null));
        }

        [Fact]
        public async void LikeAlreadyExistsTrue()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);

            var testLike = new LikeBuilder(likeRepository, _fixture.UnitOfWork, _fixture.Db).Build();

            // Act & Assert
            Assert.True(await likeRepository.LikeAlreadyExists(testLike));
        }
    }
}
