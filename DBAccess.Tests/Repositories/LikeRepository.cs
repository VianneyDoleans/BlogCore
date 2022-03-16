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
            var repository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);
            var testLikes = new Like()
            {
                User = await _fixture.Db.Users.FindAsync(1), LikeableType = LikeableType.Post,
                Post = await _fixture.Db.Posts.FindAsync(1)
            };
            await repository.AddAsync(testLikes);
            _fixture.UnitOfWork.Save();

            Assert.True(_fixture.Db.Likes.First(x => x.Post == testLikes.Post) != null);
        }

        [Fact]
        public async void AddNullLikesAsync()
        {
            var repository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.AddAsync(null));
        }

        [Fact]
        public async void GetLikesAsync()
        {
            var repository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);
            var result = await repository.GetAsync(1);

            Assert.True(result == await _fixture.Db.Likes.FindAsync(1));
        }

        [Fact]
        public async void GetLikesOutOfRangeAsync()
        {
            var repository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);

            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await repository.GetAsync(100));
        }

        [Fact]
        public async void GetAllAsync()
        {
            var repository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);
            var result = await repository.GetAllAsync();

            Assert.True(result.Count() == _fixture.Db.Likes.Count());
        }

        [Fact]
        public async void RemoveAsync()
        {
            var nbLikesAtBeginning = _fixture.Db.Likes.Count();
            var tagRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);
            var testLikes = new Like()
            {
                User = await _fixture.Db.Users.FindAsync(1),
                LikeableType = LikeableType.Post,
                Post = await _fixture.Db.Posts.FindAsync(1)
            };

            await tagRepository.AddAsync(testLikes);
            _fixture.UnitOfWork.Save();
            var nbLikesAfterAdded = _fixture.Db.Likes.Count();
            await tagRepository.RemoveAsync(testLikes);
            _fixture.UnitOfWork.Save();
            var nbLikesAfterRemoved = _fixture.Db.Likes.Count();

            Assert.True(nbLikesAtBeginning + 1 == nbLikesAfterAdded &&
                        nbLikesAfterRemoved == nbLikesAtBeginning);
        }

        [Fact]
        public async void RemoveNullAsync()
        {
            var repository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.RemoveAsync(null));
        }

        // -------

        [Fact]
        public void AddLike()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);
            var user = _fixture.Db.Users.Add(
                new User() { EmailAddress = "AddLike@email.com", Password = "1234", UserName = "AddLike" });
            var category = _fixture.Db.Categories.Add(new Category() { Name = "AddLike" });
            var post = _fixture.Db.Posts.Add(
                new Post() { Author = user.Entity, Name = "AddLike", Content = "AddLike", Category = category.Entity });
            _fixture.Db.Users.Add(user.Entity);
            _fixture.Db.SaveChanges();
            var testLike = new Like() { User = user.Entity, LikeableType = LikeableType.Post, Post = post.Entity };

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
            var user = await _fixture.Db.Users.AddAsync(
               new User() { EmailAddress = "LikeGetASpecBasic@email.com", Password = "1234", UserName = "LikeGetASpecBasic" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "LikeGetASpecBasic" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "LikeGetASpecBasic", Content = "LikeGetASpecBasic", Category = category.Entity });
            var post2 = await _fixture.Db.Posts.AddAsync(
               new Post() { Author = user.Entity, Name = "LikeGetASpecBasic2", Content = "LikeGetASpecBasic2", Category = category.Entity });
            var post3 = await _fixture.Db.Posts.AddAsync(
               new Post() { Author = user.Entity, Name = "LikeGetASpecBasic3", Content = "LikeGetASpecBasic3", Category = category.Entity });
            var post4 = await _fixture.Db.Posts.AddAsync(
               new Post() { Author = user.Entity, Name = "LikeGetASpecBasic4", Content = "LikeGetASpecBasic4", Category = category.Entity });
            _fixture.Db.Users.Add(user.Entity);
            _fixture.Db.SaveChanges();
            var testLike = new Like()
            {
                User = user.Entity,
                Post = post.Entity,
                LikeableType = LikeableType.Post
            };
            var testLike2 = new Like()
            {
                User = user.Entity,
                Post = post2.Entity,
                LikeableType = LikeableType.Post
            };
            var testLike3 = new Like()
            {
                User = user.Entity,
                Post = post3.Entity,
                LikeableType = LikeableType.Post
            };
            var testLike4 = new Like()
            {
                User = user.Entity,
                Post = post4.Entity,
                LikeableType = LikeableType.Post
            };
            likeRepository.Add(testLike);
            likeRepository.Add(testLike2);
            likeRepository.Add(testLike3);
            likeRepository.Add(testLike4);
            _fixture.UnitOfWork.Save();

            // Act
            var result = await likeRepository.GetAsync(new IdSpecification<Like>(testLike2.Id));

            // Assert
            Assert.True(result.First().Id == testLike2.Id);
        }

        [Fact]
        public async void GetAsyncWithTwoSpecifications()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);
            var user = await _fixture.Db.Users.AddAsync(
               new User() { EmailAddress = "GetAWiTwoSpecLi1@email.com", Password = "1234", UserName = "GetAWiTwoSpecLi" });
            var user2 = await _fixture.Db.Users.AddAsync(
               new User() { EmailAddress = "GetAWiTwoSpecLi2@email.com", Password = "1234", UserName = "GetAWiTwoSpecLi2" });
            var user3 = await _fixture.Db.Users.AddAsync(
               new User() { EmailAddress = "GetAWiTwoSpecLi3@email.com", Password = "1234", UserName = "GetAWiTwoSpecLi3" });
            var user4 = await _fixture.Db.Users.AddAsync(
               new User() { EmailAddress = "GetAWiTwoSpecLi4@email.com", Password = "1234", UserName = "GetAWiTwoSpecLi4" });
            var user5 = await _fixture.Db.Users.AddAsync(
               new User() { EmailAddress = "GetAWiTwoSpecLi5@email.com", Password = "1234", UserName = "GetAWiTwoSpecLi5" });
            var user6 = await _fixture.Db.Users.AddAsync(
               new User() { EmailAddress = "GetAWiTwoSpecLi6@email.com", Password = "1234", UserName = "GetAWiTwoSpecLi6" });
            await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "GetAWiTwoSpecLi7@email.com", Password = "1234", UserName = "GetAWiTwoSpecLi7" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "GetAWiTwoSpecLi" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "GetAWiTwoSpecLi", Content = "GetAWiTwoSpecLi", Category = category.Entity });
            var testLike = new Like()
            {
                User = user.Entity,
                Post = post.Entity,
                LikeableType = LikeableType.Post
            };
            var testLike2 = new Like()
            {
                User = user2.Entity,
                Post = post.Entity,
                LikeableType = LikeableType.Post
            };
            var testLike3 = new Like()
            {
                User = user3.Entity,
                Post = post.Entity,
                LikeableType = LikeableType.Post
            };
            var testLike5 = new Like()
            {
                User = user5.Entity,
                Post = post.Entity,
                LikeableType = LikeableType.Post
            };
            var testLike6 = new Like()
            {
                User = user6.Entity,
                Post = post.Entity,
                LikeableType = LikeableType.Post
            };
            var testLike4 = new Like()
            {
                User = user4.Entity,
                Post = post.Entity,
                LikeableType = LikeableType.Post
            };
            await likeRepository.AddAsync(testLike);
            await likeRepository.AddAsync(testLike2);
            await likeRepository.AddAsync(testLike3);
            await likeRepository.AddAsync(testLike4);
            await likeRepository.AddAsync(testLike5);
            await likeRepository.AddAsync(testLike6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await likeRepository.GetAsync(new UserUsernameContainsSpecification<Like>("GetAWiTwoSpecLi") & new UserUsernameContainsSpecification<Like>("4"))).ToList();

            // Assert
            Assert.True(result.Count() == 1 && result.First().User.Id == testLike4.User.Id);
        }

        [Fact]
        public async void GetAsyncWithTwoSortsAndTwoSpecificationsAndPagination()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);

            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiToSosAdTwoSpcWiPa@email.com", Password = "1234", UserName = "LiToSosAdTwoSpcWiPa" });
            var user2 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiToSosAdTwoSpcWiPa2@email.com", Password = "1234", UserName = "LiToSosAdTwoSpcWiPa2" });
            var user3 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiToSosAdTwoSpcWiPa3@email.com", Password = "1234", UserName = "LiToSosAdTwoSpcWiPa3" });
            var user4 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiToSosAdTwoSpcWiPa4@email.com", Password = "1234", UserName = "LiToSosAdTwoSpcWiPa4" });
            var user5 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiToSosAdTwoSpcWiPa5@email.com", Password = "1234", UserName = "LiToSosAdTwoSpcWiPa5" });
            var user6 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiToSosAdTwoSpcWiPa6@email.com", Password = "1234", UserName = "ALiToSosAdTwoSpcWiPe6" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "LiToSosAdTwoSpcWiPa" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "LikeTwoSortsAndTwoSpecApa", Content = "GeLiToSosAdTwoSpcWiPa", Category = category.Entity });
            var comment = await _fixture.Db.Comments.AddAsync(new Comment() {
                Author = user.Entity, Content = "GeLiToSosAdTwoSpcWiPa", PostParent = post.Entity });
            await _fixture.Db.Likes.AddAsync(
                new Like() { User = user.Entity, Post = post.Entity, LikeableType = LikeableType.Post });
            await _fixture.Db.Likes.AddAsync(
                new Like() { User = user.Entity, Comment = comment.Entity, LikeableType = LikeableType.Comment });
            var like3 = await _fixture.Db.Likes.AddAsync(
                new Like() { User = user2.Entity, Post = post.Entity, LikeableType = LikeableType.Post });
            await _fixture.Db.Likes.AddAsync(
                new Like() { User = user3.Entity, Comment = comment.Entity, LikeableType = LikeableType.Comment });
            var like5 = await _fixture.Db.Likes.AddAsync(
                new Like() { User = user4.Entity, Post = post.Entity, LikeableType = LikeableType.Post });
            await _fixture.Db.Likes.AddAsync(
                new Like() { User = user5.Entity, Post = post.Entity, LikeableType = LikeableType.Post });
            await _fixture.Db.Likes.AddAsync(
                new Like() { User = user6.Entity, Comment = comment.Entity, LikeableType = LikeableType.Comment });
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await likeRepository.GetAsync(new UserUsernameContainsSpecification<Like>("SpcWiPa")
                                                            & new PostNameSpecification<Like>("LikeTwoSortsAndTwoSpecApa"), new PagingSpecification(1, 2),
                new SortSpecification<Like>(new OrderBySpecification<Like>(x => x.User.UserName), SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count() == 2);
            Assert.Contains(result, x => x.Id == like3.Entity.Id);
            Assert.Contains(result, x => x.Id == like5.Entity.Id);
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

            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "BLiGetAWiAllArg@email.com", Password = "1234", UserName = "LiGetAWiAllArg" });
            var user2 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "ALiGetAWiAllArg2@email.com", Password = "1234", UserName = "LiGetAWiAllArg2" });
            var user3 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "ELiGetAWiAllArg3@email.com", Password = "1234", UserName = "LiGetAWiAllArg3" });
            var user4 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "GLiGetAWiAllArg4@email.com", Password = "1234", UserName = "LiGetAWiAllArg4" });
            var user5 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "ELiGetAWiAllArg5@email.com", Password = "1234", UserName = "LiGetAWiAllArg5" });
            var user6 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "ALiGetAWiAllArg6@email.com", Password = "1234", UserName = "LiGetAWiAllArg6" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "LiGetAWiAllArgCa" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "LikeTwoSortsAndTwoSpecApa", Content = "LiGetAWiAllArgPo", Category = category.Entity });
            var comment = await _fixture.Db.Comments.AddAsync(new Comment()
            {
                Author = user.Entity,
                Content = "LiGetAWiAllArgCo",
                PostParent = post.Entity
            });
            await _fixture.Db.Likes.AddAsync(
                new Like() { User = user.Entity, Post = post.Entity, LikeableType = LikeableType.Post });
            await _fixture.Db.Likes.AddAsync(
                new Like() { User = user.Entity, Comment = comment.Entity, LikeableType = LikeableType.Comment });
            await _fixture.Db.Likes.AddAsync(
                new Like() { User = user2.Entity, Post = post.Entity, LikeableType = LikeableType.Post });
            var like4 = await _fixture.Db.Likes.AddAsync(
                new Like() { User = user3.Entity, Comment = comment.Entity, LikeableType = LikeableType.Comment });
            await _fixture.Db.Likes.AddAsync(
                new Like() { User = user4.Entity, Post = post.Entity, LikeableType = LikeableType.Post });
            var like6 = await _fixture.Db.Likes.AddAsync(
                new Like() { User = user5.Entity, Post = post.Entity, LikeableType = LikeableType.Post });
            await _fixture.Db.Likes.AddAsync(
                new Like() { User = user6.Entity, Comment = comment.Entity, LikeableType = LikeableType.Comment });
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await likeRepository.GetAsync(new UserUsernameContainsSpecification<Like>("LiGet") &
                                                        new UserUsernameContainsSpecification<Like>("AllArg"),
                new PagingSpecification(1, 2),
                new SortSpecification<Like>(
                    new OrderBySpecification<Like>(x => x.User.EmailAddress),
                    SortingDirectionSpecification.Descending))).ToList();

            // Assert
            Assert.True(result.Count() == 2 && result.First().User.EmailAddress == "ELiGetAWiAllArg5@email.com");
            Assert.Contains(result, x => x.Id == like6.Entity.Id);
            Assert.Contains(result, x => x.Id == like4.Entity.Id);
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

            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiGetAWiPaSkORa@email.com", Password = "1234", UserName = "LiGetAWiPaSkORaN" });
            var user2 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiGetAWiPaSkORa2@email.com", Password = "1234", UserName = "LiGetAWiPaSkORa2" });
            var user3 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiGetAWiPaSkORa3@email.com", Password = "1234", UserName = "LiGetAWiPaSkORa3" });
            var user4 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiGetAWiPaSkORa4@email.com", Password = "1234", UserName = "LiGetAWiPaSkORa4" });
            var user5 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiGetAWiPaSkORa5@email.com", Password = "1234", UserName = "LiGetAWiPaSkORa5" });
            var user6 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiGetAWiPaSkORa6@email.com", Password = "1234", UserName = "LiGetAWiPaSkORa6" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "LiGetAWiPaSkORaCa" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "LiGetAWiPaSkORaPo", Content = "LiGetAWiPaSkORaPo", Category = category.Entity });
            var comment = await _fixture.Db.Comments.AddAsync(new Comment()
            {
                Author = user.Entity,
                Content = "LiGetAWiPaSkORaCo",
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

            var user = _fixture.Db.Users.Add(
                new User() { EmailAddress = "LiRemove@email.com", Password = "1234", UserName = "LiRemove" });
            var category = _fixture.Db.Categories.Add(new Category() { Name = "LiRemoveCa" });
            var post = _fixture.Db.Posts.Add(
                new Post() { Author = user.Entity, Name = "LiRemovePo", Content = "LiRemovePo", Category = category.Entity });
            var like = _fixture.Db.Likes.Add(
                new Like() { User = user.Entity, Post = post.Entity, LikeableType = LikeableType.Post });
            _fixture.UnitOfWork.Save();
            var nbLikesAfterAdded = _fixture.Db.Likes.Count();

            // Act
            likeRepository.Remove(like.Entity);
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

            // Act
            var result = likeRepository.Get(1);

            // Act & Assert
            Assert.True(result == _fixture.Db.Likes.Find(1));
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

            var user = _fixture.Db.Users.Add(
                new User() { EmailAddress = "LiRemoveRa@email.com", Password = "1234", UserName = "LiRemoveRa" });
            var category = _fixture.Db.Categories.Add(new Category() { Name = "LiRemoveRaCa" });
            var post = _fixture.Db.Posts.Add(
                new Post() { Author = user.Entity, Name = "LiRemoveRaPo", Content = "LiRemoveRaPo", Category = category.Entity });
            var comment = _fixture.Db.Comments.Add(new Comment()
            {
                Author = user.Entity,
                Content = "LiRemoveRaCo",
                PostParent = post.Entity
            });
            var like = _fixture.Db.Likes.Add(
                new Like() { User = user.Entity, Post = post.Entity, LikeableType = LikeableType.Post });
            var like2 = _fixture.Db.Likes.Add(
                new Like() { User = user.Entity, Comment = comment.Entity, LikeableType = LikeableType.Comment });
            _fixture.UnitOfWork.Save();
            var nbLikesAfterAdded = _fixture.Db.Likes.Count();

            // Act
            likeRepository.RemoveRange(new List<Like>() { like.Entity, like2 .Entity});
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

            var user = _fixture.Db.Users.Add(
                new User() { EmailAddress = "LiRemoveRaAs@email.com", Password = "1234", UserName = "LiRemoveRaAs" });
            var category = _fixture.Db.Categories.Add(new Category() { Name = "LiRemoveRaAsCa" });
            var post = _fixture.Db.Posts.Add(
                new Post() { Author = user.Entity, Name = "LiRemoveRaAsPo", Content = "LiRemoveRaAsPo", Category = category.Entity });
            var comment = _fixture.Db.Comments.Add(new Comment()
            {
                Author = user.Entity,
                Content = "LiRemoveRaAsCo",
                PostParent = post.Entity
            });
            var like = _fixture.Db.Likes.Add(
                new Like() { User = user.Entity, Post = post.Entity, LikeableType = LikeableType.Post });
            var like2 = _fixture.Db.Likes.Add(
                new Like() { User = user.Entity, Comment = comment.Entity, LikeableType = LikeableType.Comment });
            _fixture.UnitOfWork.Save();
            var nbLikesAfterAdded = _fixture.Db.Likes.Count();

            // Act
            await likeRepository.RemoveRangeAsync(new List<Like>() { like.Entity, like2.Entity });
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
            var category = _fixture.Db.Categories.Add(new Category() { Name = "LikeAlreadyExistsFalse" });
            var user = _fixture.Db.Users.Add(
                new User() { EmailAddress = "LikeAlreadyExistsFalse@email.com", Password = "1234", UserName = "LikeAlreadyExistsFalse" });
            var post = _fixture.Db.Posts.Add(
                new Post()
                {
                    Author = user.Entity,
                    Category = category.Entity,
                    Content = "LikeAlreadyExistsFalse",
                    Name = "LikeAlreadyExistsFalse"
                });
            var testLike = new Like()
            {
                User = user.Entity,
                LikeableType = LikeableType.Post,
                Post = post.Entity
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
            var category = _fixture.Db.Categories.Add(new Category() { Name = "LikeAlreadyExistsTrue" });
            var user = _fixture.Db.Users.Add(
                new User() { EmailAddress = "LikeAlreadyExistsTrue@email.com", Password = "1234", UserName = "LikeAlreadyExistsTrue" });
            var post = _fixture.Db.Posts.Add(
                new Post()
                {
                    Author = user.Entity, Category = category.Entity, Content = "LikeAlreadyExistsTrue",
                    Name = "LikeAlreadyExistsTrue"
                });
            var testLike = new Like()
            {
                User = user.Entity,
                LikeableType = LikeableType.Post,
                Post = post.Entity
            };
            await likeRepository.AddAsync(testLike);
            await _fixture.Db.SaveChangesAsync();

            // Act & Assert
            Assert.True(await likeRepository.LikeAlreadyExists(testLike));
        }
    }
}
