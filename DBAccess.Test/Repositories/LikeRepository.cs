using System;
using System.Linq;
using DbAccess.Data.POCO;
using DbAccess.Specifications;
using DbAccess.Specifications.FilterSpecifications.Filters;
using DbAccess.Specifications.SortSpecification;
using Xunit;

namespace DBAccess.Test.Repositories
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
                new User() { EmailAddress = "AddLike@email.com", Password = "1234", Username = "AddLike" });
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
               new User() { EmailAddress = "LikeGetASpecBasic@email.com", Password = "1234", Username = "LikeGetASpecBasic" });
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
               new User() { EmailAddress = "GetAWiTwoSpecLi1@email.com", Password = "1234", Username = "GetAWiTwoSpecLi" });
            var user2 = await _fixture.Db.Users.AddAsync(
               new User() { EmailAddress = "GetAWiTwoSpecLi2@email.com", Password = "1234", Username = "GetAWiTwoSpecLi2" });
            var user3 = await _fixture.Db.Users.AddAsync(
               new User() { EmailAddress = "GetAWiTwoSpecLi3@email.com", Password = "1234", Username = "GetAWiTwoSpecLi3" });
            var user4 = await _fixture.Db.Users.AddAsync(
               new User() { EmailAddress = "GetAWiTwoSpecLi4@email.com", Password = "1234", Username = "GetAWiTwoSpecLi4" });
            var user5 = await _fixture.Db.Users.AddAsync(
               new User() { EmailAddress = "GetAWiTwoSpecLi5@email.com", Password = "1234", Username = "GetAWiTwoSpecLi5" });
            var user6 = await _fixture.Db.Users.AddAsync(
               new User() { EmailAddress = "GetAWiTwoSpecLi6@email.com", Password = "1234", Username = "GetAWiTwoSpecLi6" });
            var user7 = await _fixture.Db.Users.AddAsync(
               new User() { EmailAddress = "GetAWiTwoSpecLi7@email.com", Password = "1234", Username = "GetAWiTwoSpecLi7" });
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
                new User() { EmailAddress = "LiToSosAdTwoSpcWiPa@email.com", Password = "1234", Username = "LiToSosAdTwoSpcWiPa" });
            var user2 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiToSosAdTwoSpcWiPa2@email.com", Password = "1234", Username = "LiToSosAdTwoSpcWiPa2" });
            var user3 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiToSosAdTwoSpcWiPa3@email.com", Password = "1234", Username = "LiToSosAdTwoSpcWiPa3" });
            var user4 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiToSosAdTwoSpcWiPa4@email.com", Password = "1234", Username = "LiToSosAdTwoSpcWiPa4" });
            var user5 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiToSosAdTwoSpcWiPa5@email.com", Password = "1234", Username = "LiToSosAdTwoSpcWiPa5" });
            var user6 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiToSosAdTwoSpcWiPa6@email.com", Password = "1234", Username = "ALiToSosAdTwoSpcWiPe6" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "LiToSosAdTwoSpcWiPa" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "LikeTwoSortsAndTwoSpecApa", Content = "GeLiToSosAdTwoSpcWiPa", Category = category.Entity });
            var comment = await _fixture.Db.Comments.AddAsync(new Comment() {
                Author = user.Entity, Content = "GeLiToSosAdTwoSpcWiPa", PostParent = post.Entity });
            var like = await _fixture.Db.Likes.AddAsync(
                new Like() { User = user.Entity, Post = post.Entity, LikeableType = LikeableType.Post });
            var like2 = await _fixture.Db.Likes.AddAsync(
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
            var result = (await likeRepository.GetAsync(new UserUsernameContainsSpecification<Like>("SpcWiPa")
                                                            & new PostNameSpecification<Like>("LikeTwoSortsAndTwoSpecApa"), new PagingSpecification(1, 2),
                new SortSpecification<Like>(new OrderBySpecification<Like>(x => x.User.Username), SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count() == 2);
            Assert.Contains(result, x => x.Id == like3.Entity.Id || x.Id == like5.Entity.Id);
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
                new User() { EmailAddress = "LiToSosAdTwoSpcWiPa@email.com", Password = "1234", Username = "LiToSosAdTwoSpcWiPa" });
            var user2 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiToSosAdTwoSpcWiPa2@email.com", Password = "1234", Username = "LiToSosAdTwoSpcWiPa2" });
            var user3 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiToSosAdTwoSpcWiPa3@email.com", Password = "1234", Username = "LiToSosAdTwoSpcWiPa3" });
            var user4 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiToSosAdTwoSpcWiPa4@email.com", Password = "1234", Username = "LiToSosAdTwoSpcWiPa4" });
            var user5 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiToSosAdTwoSpcWiPa5@email.com", Password = "1234", Username = "LiToSosAdTwoSpcWiPa5" });
            var user6 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LiToSosAdTwoSpcWiPa6@email.com", Password = "1234", Username = "ALiToSosAdTwoSpcWiPe6" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "LiToSosAdTwoSpcWiPa" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "LikeTwoSortsAndTwoSpecApa", Content = "GeLiToSosAdTwoSpcWiPa", Category = category.Entity });
            var comment = await _fixture.Db.Comments.AddAsync(new Comment()
            {
                Author = user.Entity,
                Content = "GeLiToSosAdTwoSpcWiPa",
                PostParent = post.Entity
            });
            var like = await _fixture.Db.Likes.AddAsync(
                new Like() { User = user.Entity, Post = post.Entity, LikeableType = LikeableType.Post });
            var like2 = await _fixture.Db.Likes.AddAsync(
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
            var result = (await likeRepository.GetAsync(new ContentContainsSpecification<Like>("AK") &
                new ContentContainsSpecification<Like>("WithAllArguments"),
                new PagingSpecification(0, 2),
                new SortSpecification<Like>(
                    new OrderBySpecification<Like>(x => x.Content),
                    SortingDirectionSpecification.Descending))).ToList();

            // Assert
            Assert.True(result.Count() == 2 && result.First().Content == testLike5.Content);
        }

        [Fact]
        public async void GetAsyncWithPagination()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LikeGetAsyncWithPagination@email.com", Password = "1234", Username = "GetAsyncWithPagination" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "GetAsyncWithPagination" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "GetAsyncWithPagination", Content = "GetAsyncWithPagination", Category = category.Entity });
            var testLike = new Like()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "Like1GetAsyncWithPagination1"
            };
            var testLike2 = new Like()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "Like1GetAsyncWithPagination2"
            };
            var testLike3 = new Like()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "Like1GetAsyncWithPagination3"
            };
            var testLike4 = new Like()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "Like1GetAsyncWithPagination4"
            };
            var testLike5 = new Like()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "Like1GetAsyncWithPagination5"
            };
            var testLike6 = new Like()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "Like1GetAsyncWithPagination6"
            };
            await likeRepository.AddAsync(testLike);
            await likeRepository.AddAsync(testLike2);
            await likeRepository.AddAsync(testLike3);
            await likeRepository.AddAsync(testLike4);
            await likeRepository.AddAsync(testLike5);
            await likeRepository.AddAsync(testLike6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await likeRepository.GetAsync(new ContentContainsSpecification<Like>("1GetAsyncWithPagination"),
                new PagingSpecification(2, 3),
                new SortSpecification<Like>(
                    new OrderBySpecification<Like>(x => x.Content),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count() == 3);
            Assert.Contains(result, x => x.Id == testLike3.Id &&
                                         x.Content == testLike3.Content);
            Assert.Contains(result, x => x.Id == testLike4.Id &&
                                         x.Content == testLike4.Content);
            Assert.Contains(result, x => x.Id == testLike5.Id &&
                                         x.Content == testLike5.Content);
        }

        [Fact]
        public async void GetAsyncWithPaginationTakeOutOfRange()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "CoGetAWithPagTaOfRg@email.com", Password = "1234", Username = "CoGetAWithPagTaOfRg" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "CoGetAWithPagTaOfRg" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "CoGetAWithPagTaOfRg", Content = "CoGetAWithPagTaOfRg", Category = category.Entity });
            var testLike = new Like()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "LikeGetAsyncWithPaginationTakeOutOfRange1"
            };
            var testLike2 = new Like()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "LikeGetAsyncWithPaginationTakeOutOfRange2"
            };
            var testLike3 = new Like()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "LikeGetAsyncWithPaginationTakeOutOfRange3"
            };
            var testLike5 = new Like()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "LikeGetAsyncWithPaginationTakeOutOfRange4"
            };
            var testLike6 = new Like()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "LikeGetAsyncWithPaginationTakeOutOfRange5"
            };
            var testLike4 = new Like()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "LikeGetAsyncWithPaginationTakeOutOfRange6"
            };
            await likeRepository.AddAsync(testLike);
            await likeRepository.AddAsync(testLike2);
            await likeRepository.AddAsync(testLike3);
            await likeRepository.AddAsync(testLike4);
            await likeRepository.AddAsync(testLike5);
            await likeRepository.AddAsync(testLike6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await likeRepository.GetAsync(new ContentContainsSpecification<Like>("GetAsyncWithPaginationTakeOutOfRange"),
                new PagingSpecification(2, 22),
                new SortSpecification<Like>(
                    new OrderBySpecification<Like>(x => x.Content),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count() == 4);
            Assert.Contains(result, x => x.Id == testLike3.Id &&
                                         x.Content == testLike3.Content);
            Assert.Contains(result, x => x.Id == testLike4.Id &&
                                         x.Content == testLike4.Content);
            Assert.Contains(result, x => x.Id == testLike5.Id &&
                                         x.Content == testLike5.Content);
            Assert.Contains(result, x => x.Id == testLike6.Id &&
                                         x.Content == testLike6.Content);
        }

        [Fact]
        public async void GetAsyncWithPaginationTakeNegative()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LikeGetAsyncWithPaginationTakeNegative@email.com", Password = "1234", Username = "ComGetAWithPagTakeNega" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "ComGetAWithAllArgs" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "ComGetAWithAllArgs", Content = "ComGetAWithAllArgs", Category = category.Entity });
            var testLike = new Like()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "LikeGetAsyncWithPaginationTakeNegative1"
            };
            var testLike2 = new Like()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "LikeGetAsyncWithPaginationTakeNegative2"
            };
            var testLike3 = new Like()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "LikeGetAsyncWithPaginationTakeNegative3"
            };
            var testLike5 = new Like()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "LikeGetAsyncWithPaginationTakeNegative4"
            };
            var testLike6 = new Like()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "LikeGetAsyncWithPaginationTakeNegative5"
            };
            var testLike4 = new Like()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "LikeGetAsyncWithPaginationTakeNegative6"
            };
            await likeRepository.AddAsync(testLike);
            await likeRepository.AddAsync(testLike2);
            await likeRepository.AddAsync(testLike3);
            await likeRepository.AddAsync(testLike4);
            await likeRepository.AddAsync(testLike5);
            await likeRepository.AddAsync(testLike6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await likeRepository.GetAsync(new ContentContainsSpecification<Like>("GetAsyncWithPaginationTakeNegative"),
                new PagingSpecification(2, -2),
                new SortSpecification<Like>(
                    new OrderBySpecification<Like>(x => x.Content),
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
                new User() { EmailAddress = "LikeGetAsyncWithPaginationSkipNegative@email.com", Password = "1234", Username = "ComGetAWithPagSkipNega" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "ComGetAWithPagSkipNega" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "ComGetAWithPagSkipNega", Content = "ComGetAWithPagSkipNega", Category = category.Entity });
            var testLike = new Like()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "LikeGetAsyncWithPaginationSkipNegative"
            };
            var testLike2 = new Like()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "LikeGetAsyncWithPaginationSkipNegative2"
            };
            var testLike3 = new Like()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "LikeGetAsyncWithPaginationSkipNegative3"
            };
            var testLike4 = new Like()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "LikeGetAsyncWithPaginationSkipNegative4"
            };
            var testLike5 = new Like()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "LikeGetAsyncWithPaginationSkipSkipNegative5"
            };
            var testLike6 = new Like()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "LikeGetAsyncWithPaginationSkipSkipNegative6"
            };
            await likeRepository.AddAsync(testLike);
            await likeRepository.AddAsync(testLike2);
            await likeRepository.AddAsync(testLike3);
            await likeRepository.AddAsync(testLike4);
            await likeRepository.AddAsync(testLike5);
            await likeRepository.AddAsync(testLike6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await likeRepository.GetAsync(new ContentContainsSpecification<Like>("GetAsyncWithPaginationSkipNegative"),
                new PagingSpecification(-2, 3),
                new SortSpecification<Like>(
                    new OrderBySpecification<Like>(x => x.Content),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count() == 3);
            Assert.Contains(result, x => x.Id == testLike.Id &&
                                         x.Content == testLike.Content);
            Assert.Contains(result, x => x.Id == testLike2.Id &&
                                         x.Content == testLike2.Content);
            Assert.Contains(result, x => x.Id == testLike3.Id &&
                                         x.Content == testLike3.Content);
        }

        [Fact]
        public async void GetAsyncWithPaginationSkipOutOfRange()
        {
            // Arrange
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "LikeGetAsyncWithPaginationSkipOutOfRange@email.com", Password = "1234", Username = "ComGetAWiPagSkipOfRa" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "ComGetAWiPagSkipOfRa" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "ComGetAWiPagSkipOfRa", Content = "ComGetAWiPagSkipOfRa", Category = category.Entity });
            var testLike = new Like()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "LikeGetAsyncWithPaginationSkipOutOfRange"
            };
            var testLike2 = new Like()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "LikeGetAsyncWithPaginationSkipOutOfRange2"
            };
            var testLike3 = new Like()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "LikeGetAsyncWithPaginationSkipOutOfRange3"
            };
            var testLike5 = new Like()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "LikeGetAsyncWithPaginationSkipOutOfRange4"
            };
            var testLike6 = new Like()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "LikeGetAsyncWithPaginationSkipOutOfRange5"
            };
            var testLike4 = new Like()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "LikeGetAsyncWithPaginationSkipOutOfRange6"
            };
            await likeRepository.AddAsync(testLike);
            await likeRepository.AddAsync(testLike2);
            await likeRepository.AddAsync(testLike3);
            await likeRepository.AddAsync(testLike4);
            await likeRepository.AddAsync(testLike5);
            await likeRepository.AddAsync(testLike6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await likeRepository.GetAsync(new ContentContainsSpecification<Like>("GetAsyncWithPaginationSkipOutOfRange"),
                new PagingSpecification(7, 3),
                new SortSpecification<Like>(
                    new OrderBySpecification<Like>(x => x.Content),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(!result.Any());
        }

        [Fact]
        public void Remove()
        {
            // Arrange
            var nbCategoriesAtBeginning = _fixture.Db.Likes.Count();
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);
            var user = _fixture.Db.Users.Add(
                new User() { EmailAddress = "LikeRemove@email.com", Password = "1234", Username = "LikeRemove" });
            var category = _fixture.Db.Categories.Add(new Category() { Name = "CLikeGetRemove" });
            var post = _fixture.Db.Posts.Add(
                new Post() { Author = user.Entity, Name = "LikeRemove", Content = "CommenRemove", Category = category.Entity });
            var testLike = new Like()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "Remove"
            };
            likeRepository.Add(testLike);
            _fixture.UnitOfWork.Save();
            var nbCategoriesAfterAdded = _fixture.Db.Likes.Count();

            // Act
            likeRepository.Remove(testLike);
            _fixture.UnitOfWork.Save();

            // Assert
            var nbCategoriesAfterRemoved = _fixture.Db.Likes.Count();
            Assert.True(nbCategoriesAtBeginning + 1 == nbCategoriesAfterAdded &&
                        nbCategoriesAfterRemoved == nbCategoriesAtBeginning);
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
            var nbCategoriesAtBeginning = _fixture.Db.Likes.Count();
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);
            var category = _fixture.Db.Categories.Add(new Category() { Name = "LikeRemoveRange" });
            var user = _fixture.Db.Users.Add(
                new User() { EmailAddress = "LikeRemoveRange@email.com", Password = "1234", Username = "LikeGetRemoveRg" });
            var post = _fixture.Db.Posts.Add(
                new Post() { Author = user.Entity, Name = "LikeRemoveRange", Content = "LikeRemoveRange", Category = category.Entity });
            var testLike = new Like()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "LikeRemoveRange1"
            };
            var testLike2 = new Like()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "LikeRemoveRange2"
            };
            likeRepository.Add(testLike);
            likeRepository.Add(testLike2);
            _fixture.UnitOfWork.Save();
            var nbCategoriesAfterAdded = _fixture.Db.Likes.Count();

            // Act
            likeRepository.RemoveRange(new List<Like>() { testLike, testLike2 });
            _fixture.UnitOfWork.Save();

            // Assert
            var nbCategoriesAfterRemoved = _fixture.Db.Likes.Count();
            Assert.True(nbCategoriesAtBeginning + 2 == nbCategoriesAfterAdded &&
                        nbCategoriesAfterRemoved == nbCategoriesAtBeginning);
        }

        [Fact]
        public async void RemoveRangeAsync()
        {
            // Arrange
            var nbCategoriesAtBeginning = _fixture.Db.Likes.Count();
            var likeRepository = new DbAccess.Repositories.Like.LikeRepository(_fixture.Db);
            var category = _fixture.Db.Categories.Add(new Category() { Name = "LikeRemoveRange" });
            var user = _fixture.Db.Users.Add(
                new User() { EmailAddress = "RemoveRangeAsync@email.com", Password = "1234", Username = "ComRemoveRgAsync" });
            var post = _fixture.Db.Posts.Add(
                new Post() { Author = user.Entity, Name = "ComRemoveRgAsync", Content = "ComRemoveRgAsync", Category = category.Entity });
            var testLike = new Like()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "LikeRemoveRangeAsync1"
            };
            var testLike2 = new Like()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "LikeRemoveRangeAsync2"
            };
            await likeRepository.AddAsync(testLike);
            await likeRepository.AddAsync(testLike2);
            _fixture.UnitOfWork.Save();
            var nbCategoriesAfterAdded = _fixture.Db.Likes.Count();

            // Act
            await likeRepository.RemoveRangeAsync(new List<Like>() { testLike, testLike2 });
            _fixture.UnitOfWork.Save();

            // Assert
            var nbCategoriesAfterRemoved = _fixture.Db.Likes.Count();
            Assert.True(nbCategoriesAtBeginning + 2 == nbCategoriesAfterAdded &&
                        nbCategoriesAfterRemoved == nbCategoriesAtBeginning);
        }
    }
}
