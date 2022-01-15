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
    public class PostsRepository
    {
        private readonly DatabaseFixture _fixture;

        public PostsRepository()
        {
            _fixture = new DatabaseFixture();
        }

        [Fact]
        public async void AddPostsAsync()
        {
            var repository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var testPosts = new Post() { Name = "This is a test for post" };
            await repository.AddAsync(testPosts);
            _fixture.UnitOfWork.Save();

            Assert.True(_fixture.Db.Posts.First(x => x.Name == testPosts.Name) != null);
        }

        [Fact]
        public async void AddNullPostsAsync()
        {
            var repository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.AddAsync(null));
        }

        [Fact]
        public async void GetPostsAsync()
        {
            var repository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var result = await repository.GetAsync(1);

            Assert.True(result == await _fixture.Db.Posts.FindAsync(1));
        }

        [Fact]
        public async void GetPostsOutOfRangeAsync()
        {
            var repository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);

            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await repository.GetAsync(100));
        }

        [Fact]
        public async void GetAllAsync()
        {
            var repository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var result = await repository.GetAllAsync();

            Assert.True(result.Count() == _fixture.Db.Posts.Count());
        }

        [Fact]
        public async void RemoveAsync()
        {
            var nbPostsAtBeginning = _fixture.Db.Posts.Count();
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var testPosts = new Post() { Name = "This is a test post" };

            await postRepository.AddAsync(testPosts);
            _fixture.UnitOfWork.Save();
            var nbPostsAfterAdded = _fixture.Db.Posts.Count();
            await postRepository.RemoveAsync(testPosts);
            _fixture.UnitOfWork.Save();
            var nbPostsAfterRemoved = _fixture.Db.Posts.Count();

            Assert.True(nbPostsAtBeginning + 1 == nbPostsAfterAdded &&
                        nbPostsAfterRemoved == nbPostsAtBeginning);
        }

        [Fact]
        public async void RemoveNullAsync()
        {
            var repository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.RemoveAsync(null));
        }

        // --------

        [Fact]
        public void AddPost()
        {
            // Arrange
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var user = _fixture.Db.Users.Add(
                new User() { EmailAddress = "AddPost@email.com", Password = "1234", Username = "AddPost" });
            var category = _fixture.Db.Categories.Add(new Category() { Name = "AddPost" });
            _fixture.Db.SaveChanges();
            var testPost = new Post() { Author = user.Entity, Name = "AddPost", Content = "AddPost", Category = category.Entity };

            // Act
            postRepository.Add(testPost);
            _fixture.UnitOfWork.Save();

            // Assert
            Assert.True(_fixture.Db.Posts.First(x => x.Content == "AddPost") != null);
        }

        [Fact]
        public void AddNullPost()
        {
            // Arrange
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                postRepository.Add(null);
                _fixture.Db.SaveChanges();
            });
        }

        [Fact]
        public void CountAll()
        {
            // Arrange
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);

            // Act & Assert
            Assert.True(_fixture.Db.Posts.Count() == postRepository.CountAll());
        }

        [Fact]
        public async void CountAllAsync()
        {
            // Arrange
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);

            // Act & Assert
            Assert.True(_fixture.Db.Posts.Count() == await postRepository.CountAllAsync());
        }


        [Fact]
        public void GetAll()
        {
            // Arrange
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);

            // Act
            var result = postRepository.GetAll();

            // Assert
            Assert.True(result.Count() == _fixture.Db.Posts.Count());
        }

        [Fact]
        public async void GetAsyncSpecificationBasic()
        {
            // Arrange
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var user = await _fixture.Db.Users.AddAsync(
               new User() { EmailAddress = "GetASpecBasicPost@email.com", Password = "1234", Username = "GetASpecBasicPost" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "GetASpecBasicPost" });
            _fixture.Db.Users.Add(user.Entity);
            _fixture.Db.SaveChanges();
            var testPost = new Post()
            {
                Author = user.Entity,
                Name = "PostGetAsyncSpecification",
                Category = category.Entity,
                Content = "GetAsyncSpecification"
            };
            var testPost2 = new Post()
            {
                Author = user.Entity,
                Name = "PostGetAsyncSpecification2",
                Category = category.Entity,
                Content = "GetAsyncSpecification2"
            };
            var testPost3 = new Post()
            {
                Author = user.Entity,
                Name = "PostGetAsyncSpecification3",
                Category = category.Entity,
                Content = "GetAsyncSpecification3"
            };
            var testPost4 = new Post()
            {
                Author = user.Entity,
                Name = "PostGetAsyncSpecification4",
                Category = category.Entity,
                Content = "GetAsyncSpecification4"
            };
            postRepository.Add(testPost);
            postRepository.Add(testPost2);
            postRepository.Add(testPost3);
            postRepository.Add(testPost4);
            _fixture.UnitOfWork.Save();

            // Act
            var result = await postRepository.GetAsync(new IdSpecification<Post>(testPost2.Id));

            // Assert
            Assert.True(result.First().Id == testPost2.Id);
        }

        [Fact]
        public async void GetAsyncWithTwoSpecifications()
        {
            // Arrange
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var user = await _fixture.Db.Users.AddAsync(
               new User() { EmailAddress = "GetAWiTwoSpecPost@email.com", Password = "1234", Username = "GetAWiTwoSpecPost" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "GetAWiTwoSpecPost" });
            _fixture.Db.SaveChanges();
            var testPost = new Post()
            {
                Author = user.Entity,
                Name = "GetAWiTwoSpecPost",
                Category = category.Entity,
                Content = "PostGetAsyncABSpecification"
            };
            var testPost2 = new Post()
            {
                Author = user.Entity,
                Name = "GetAWiTwoSpecPost2",
                Category = category.Entity,
                Content = "PostGetAsyncAUSpecification2"
            };
            var testPost3 = new Post()
            {
                Author = user.Entity,
                Name = "GetAWiTwoSpecPost3",
                Category = category.Entity,
                Content = "PostGetAsyncAKSpecification3"
            };
            var testPost4 = new Post()
            {
                Author = user.Entity,
                Name = "GetAWiTwoSpecPost4",
                Category = category.Entity,
                Content = "PostGetAsyncAKSpecification3164"
            };
            var testPost5 = new Post()
            {
                Author = user.Entity,
                Name = "GetAWiTwoSpecPost5",
                Category = category.Entity,
                Content = "PostGetAsyncAKSpecification32"
            };
            var testPost6 = new Post()
            {
                Author = user.Entity,
                Name = "GetAWiTwoSpecPost6",
                Category = category.Entity,
                Content = "PostGetAsyncAWSpecification6"
            };
            await postRepository.AddAsync(testPost);
            await postRepository.AddAsync(testPost2);
            await postRepository.AddAsync(testPost3);
            await postRepository.AddAsync(testPost4);
            await postRepository.AddAsync(testPost5);
            await postRepository.AddAsync(testPost6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await postRepository.GetAsync(new ContentContainsSpecification<Post>("AK") & new ContentContainsSpecification<Post>("164"))).ToList();

            // Assert
            Assert.True(result.Count() == 1 && result.First().Content == testPost4.Content);
        }
        
        [Fact]
        public async void GetAsyncWithTwoSortsAndTwoSpecificationsAndPagination()
        {
            // Arrange
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);

            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "PostTwoSortsAndTwoSpec@email.com", Password = "1234", Username = "TwoSortsAndTwoSpecPst" });
            var user2 = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "PostAnotherTwoSortsAndTwoSpec@email.com", Password = "1234", Username = "TwoSortsAndTwoSpecPst2" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "TwoSortsAndTwoSpecPst" });
            await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "TwwooGetAsyncWithTwoSortsPost", Category = category.Entity, Content = "TwwooGetAsyncWithTwoSortsPost" });
            await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user2.Entity, Name = "GetAsyncWithTwoSorts2Post", Category = category.Entity, Content = "GetAsyncWithTwoSorts2Post" });
            await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "GetAsyncWithTwoSorts3TwwooPost", Category = category.Entity, Content = "GetAsyncWithTwoSorts3TwwooPost" });
            var post4 = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user2.Entity, Name = "AGetTwwooAsyncWithTwoSorts4Post", Category = category.Entity, Content = "AGetTwwooAsyncWithTwoSorts4Post" });
            await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user2.Entity, Name = "GetAsyncTwwooWithTwoSorts5Post", Category = category.Entity, Content = "GetAsyncTwwooWithTwoSorts5Post" });
            await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "GetAsyncWithTwoSorts6Post", Category = category.Entity, Content = "GetAsyncWithTwoSorts6Post" });
            await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "TGetAsyncWithTwoorts7Post", Category = category.Entity, Content = "TwwooGetAsyncWithTwoorts7Post" });
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await postRepository.GetAsync(new ContentContainsSpecification<Post>("WithTwoSorts")
                                                            & new ContentContainsSpecification<Post>("Twwoo"), new PagingSpecification(0, 20),
                new SortSpecification<Post>(new OrderBySpecification<Post>(x => x.Author.Username), SortingDirectionSpecification.Descending) &
                new SortSpecification<Post>(new OrderBySpecification<Post>(x => x.Content), SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count() == 4 && result.First().Content == post4.Entity.Content);
        }

        [Fact]
        public async void GetAsyncWithNoArgument()
        {
            // Arrange
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);

            // Act & Assert
            Assert.True((await postRepository.GetAsync()).ToList().Count() == _fixture.Db.Posts.Count());
        }

        [Fact]
        public async void GetAsyncWithAllArguments()
        {
            // Arrange
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "PostGetAsyncWithAllArguments@email.com", Password = "1234", Username = "PostGetAWithAllArgs" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "PostGetAWithAllArgs" });
            var testPost = new Post()
            {
                Author = user.Entity,
                Name = "PostGetAsyncABSpecificationWithAllArguments", Category = category.Entity,
                Content = "PostGetAsyncABSpecificationWithAllArguments"
            };
            var testPost2 = new Post()
            {
                Author = user.Entity,
                Name = "PostGetAsyncAUSpecification2WithAllArguments", Category = category.Entity,
                Content = "PostGetAsyncAUSpecification2WithAllArguments"
            };
            var testPost3 = new Post()
            {
                Author = user.Entity,
                Name = "PostGetAsyncAKSpecification3WithAllArguments", Category = category.Entity,
                Content = "PostGetAsyncAKSpecification3WithAllArguments"
            };
            var testPost4 = new Post()
            {
                Author = user.Entity,
                Name = "PostGetAsyncAKSpecification3WithAllArguments163", Category = category.Entity,
                Content = "PostGetAsyncAKSpecification3WithAllArguments163"
            };
            var testPost5 = new Post()
            {
                Author = user.Entity,
                Name = "PostGetAsyncAKSpecification3WithAllArguments2", Category = category.Entity,
                Content = "PostGetAsyncAKSpecification3WithAllArguments2"
            };
            var testPost6 = new Post()
            {
                Author = user.Entity,
                Name = "PostGetAsyncAWSpecification4WithAllArguments", Category = category.Entity,
                Content = "PostGetAsyncAWSpecification4WithAllArguments"
            };
            await postRepository.AddAsync(testPost);
            await postRepository.AddAsync(testPost2);
            await postRepository.AddAsync(testPost3);
            await postRepository.AddAsync(testPost4);
            await postRepository.AddAsync(testPost5);
            await postRepository.AddAsync(testPost6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await postRepository.GetAsync(new ContentContainsSpecification<Post>("AK") &
                new ContentContainsSpecification<Post>("WithAllArguments"),
                new PagingSpecification(0, 2),
                new SortSpecification<Post>(
                    new OrderBySpecification<Post>(x => x.Content),
                    SortingDirectionSpecification.Descending))).ToList();

            // Assert
            Assert.True(result.Count() == 2 && result.First().Content == testPost5.Content);
        }

        [Fact]
        public async void GetAsyncWithPagination()
        {
            // Arrange
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "PostGetAsyncWithPagination@email.com", Password = "1234", Username = "PostGetAsyncWithPagination" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "PostGetAsyncWithPagination" });
            await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "PostGetAsyncWithPagination", Content = "PostGetAsyncWithPagination", Category = category.Entity });
            var testPost = new Post()
            {
                Author = user.Entity,
                Name = "Post1GetAsyncWithPagination1", Category = category.Entity,
                Content = "Post1GetAsyncWithPagination1"
            };
            var testPost2 = new Post()
            {
                Author = user.Entity,
                Name = "Post1GetAsyncWithPagination2", Category = category.Entity,
                Content = "Post1GetAsyncWithPagination2"
            };
            var testPost3 = new Post()
            {
                Author = user.Entity,
                Name = "Post1GetAsyncWithPagination3", Category = category.Entity,
                Content = "Post1GetAsyncWithPagination3"
            };
            var testPost4 = new Post()
            {
                Author = user.Entity,
                Name = "Post1GetAsyncWithPagination4", Category = category.Entity,
                Content = "Post1GetAsyncWithPagination4"
            };
            var testPost5 = new Post()
            {
                Author = user.Entity,
                Name = "Post1GetAsyncWithPagination5", Category = category.Entity,
                Content = "Post1GetAsyncWithPagination5"
            };
            var testPost6 = new Post()
            {
                Author = user.Entity,
                Name = "Post1GetAsyncWithPagination6", Category = category.Entity,
                Content = "Post1GetAsyncWithPagination6"
            };
            await postRepository.AddAsync(testPost);
            await postRepository.AddAsync(testPost2);
            await postRepository.AddAsync(testPost3);
            await postRepository.AddAsync(testPost4);
            await postRepository.AddAsync(testPost5);
            await postRepository.AddAsync(testPost6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await postRepository.GetAsync(new ContentContainsSpecification<Post>("1GetAsyncWithPagination"),
                new PagingSpecification(2, 3),
                new SortSpecification<Post>(
                    new OrderBySpecification<Post>(x => x.Content),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count() == 3);
            Assert.Contains(result, x => x.Id == testPost3.Id &&
                                         x.Content == testPost3.Content);
            Assert.Contains(result, x => x.Id == testPost4.Id &&
                                         x.Content == testPost4.Content);
            Assert.Contains(result, x => x.Id == testPost5.Id &&
                                         x.Content == testPost5.Content);
        }

        [Fact]
        public async void GetAsyncWithPaginationTakeOutOfRange()
        {
            // Arrange
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "PostGetAWithPagTaOfRg@email.com", Password = "1234", Username = "PostGetAWithPagTaOfRg" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "PostGetAWithPagTaOfRg" });
            var testPost = new Post()
            {
                Author = user.Entity,
                Name = "PostGetAsyncWithPaginationTakeOutOfRange1", Category = category.Entity,
                Content = "PostGetAsyncWithPaginationTakeOutOfRange1"
            };
            var testPost2 = new Post()
            {
                Author = user.Entity,
                Name = "PostGetAsyncWithPaginationTakeOutOfRange2", Category = category.Entity,
                Content = "PostGetAsyncWithPaginationTakeOutOfRange2"
            };
            var testPost3 = new Post()
            {
                Author = user.Entity,
                Name = "PostGetAsyncWithPaginationTakeOutOfRange3", Category = category.Entity,
                Content = "PostGetAsyncWithPaginationTakeOutOfRange3"
            };
            var testPost5 = new Post()
            {
                Author = user.Entity,
                Name = "PostGetAsyncWithPaginationTakeOutOfRange4", Category = category.Entity,
                Content = "PostGetAsyncWithPaginationTakeOutOfRange4"
            };
            var testPost6 = new Post()
            {
                Author = user.Entity,
                Name = "PostGetAsyncWithPaginationTakeOutOfRange5", Category = category.Entity,
                Content = "PostGetAsyncWithPaginationTakeOutOfRange5"
            };
            var testPost4 = new Post()
            {
                Author = user.Entity,
                Name = "PostGetAsyncWithPaginationTakeOutOfRange6", Category = category.Entity,
                Content = "PostGetAsyncWithPaginationTakeOutOfRange6"
            };
            await postRepository.AddAsync(testPost);
            await postRepository.AddAsync(testPost2);
            await postRepository.AddAsync(testPost3);
            await postRepository.AddAsync(testPost4);
            await postRepository.AddAsync(testPost5);
            await postRepository.AddAsync(testPost6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await postRepository.GetAsync(new ContentContainsSpecification<Post>("GetAsyncWithPaginationTakeOutOfRange"),
                new PagingSpecification(2, 22),
                new SortSpecification<Post>(
                    new OrderBySpecification<Post>(x => x.Content),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count() == 4);
            Assert.Contains(result, x => x.Id == testPost3.Id &&
                                         x.Content == testPost3.Content);
            Assert.Contains(result, x => x.Id == testPost4.Id &&
                                         x.Content == testPost4.Content);
            Assert.Contains(result, x => x.Id == testPost5.Id &&
                                         x.Content == testPost5.Content);
            Assert.Contains(result, x => x.Id == testPost6.Id &&
                                         x.Content == testPost6.Content);
        }

        [Fact]
        public async void GetAsyncWithPaginationTakeNegative()
        {
            // Arrange
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "PostGetAsyncWithPaginationTakeNegative@email.com", Password = "1234", Username = "ComGetAWithPagTakeNega" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "PostGetAWithAllArgs" });
            var testPost = new Post()
            {
                Author = user.Entity,
                Name = "PostGetAsyncWithPaginationTakeNegative1", Category = category.Entity,
                Content = "PostGetAsyncWithPaginationTakeNegative1"
            };
            var testPost2 = new Post()
            {
                Author = user.Entity,
                Name = "PostGetAsyncWithPaginationTakeNegative2", Category = category.Entity,
                Content = "PostGetAsyncWithPaginationTakeNegative2"
            };
            var testPost3 = new Post()
            {
                Author = user.Entity,
                Name = "PostGetAsyncWithPaginationTakeNegative3", Category = category.Entity,
                Content = "PostGetAsyncWithPaginationTakeNegative3"
            };
            var testPost5 = new Post()
            {
                Author = user.Entity,
                Name = "PostGetAsyncWithPaginationTakeNegative4", Category = category.Entity,
                Content = "PostGetAsyncWithPaginationTakeNegative4"
            };
            var testPost6 = new Post()
            {
                Author = user.Entity,
                Name = "PostGetAsyncWithPaginationTakeNegative5", Category = category.Entity,
                Content = "PostGetAsyncWithPaginationTakeNegative5"
            };
            var testPost4 = new Post()
            {
                Author = user.Entity,
                Name = "PostGetAsyncWithPaginationTakeNegative6", Category = category.Entity,
                Content = "PostGetAsyncWithPaginationTakeNegative6"
            };
            await postRepository.AddAsync(testPost);
            await postRepository.AddAsync(testPost2);
            await postRepository.AddAsync(testPost3);
            await postRepository.AddAsync(testPost4);
            await postRepository.AddAsync(testPost5);
            await postRepository.AddAsync(testPost6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await postRepository.GetAsync(new ContentContainsSpecification<Post>("GetAsyncWithPaginationTakeNegative"),
                new PagingSpecification(2, -2),
                new SortSpecification<Post>(
                    new OrderBySpecification<Post>(x => x.Content),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(!result.Any());
        }

        [Fact]
        public async void GetAsyncWithPaginationSkipNegative()
        {
            // Arrange
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "PostGetAsyncWithPaginationSkipNegative@email.com", Password = "1234", Username = "ComGetAWithPagSkipNega" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "ComGetAWithPagSkipNega" });
            await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "PostGetAWithPagSkipNega", Content = "PostGetAWithPagSkipNega", Category = category.Entity });
            var testPost = new Post()
            {
                Author = user.Entity,
                Name = "PostGetAsyncWithPaginationSkipNegative", Category = category.Entity,
                Content = "PostGetAsyncWithPaginationSkipNegative"
            };
            var testPost2 = new Post()
            {
                Author = user.Entity,
                Name = "PostGetAsyncWithPaginationSkipNegative2", Category = category.Entity,
                Content = "PostGetAsyncWithPaginationSkipNegative2"
            };
            var testPost3 = new Post()
            {
                Author = user.Entity,
                Name = "PostGetAsyncWithPaginationSkipNegative3", Category = category.Entity,
                Content = "PostGetAsyncWithPaginationSkipNegative3"
            };
            var testPost4 = new Post()
            {
                Author = user.Entity,
                Name = "PostGetAsyncWithPaginationSkipNegative4", Category = category.Entity,
                Content = "PostGetAsyncWithPaginationSkipNegative4"
            };
            var testPost5 = new Post()
            {
                Author = user.Entity,
                Name = "PostGetAsyncWithPaginationSkipSkipNegative5", Category = category.Entity,
                Content = "PostGetAsyncWithPaginationSkipSkipNegative5"
            };
            var testPost6 = new Post()
            {
                Author = user.Entity,
                Name = "PostGetAsyncWithPaginationSkipSkipNegative6", Category = category.Entity,
                Content = "PostGetAsyncWithPaginationSkipSkipNegative6"
            };
            await postRepository.AddAsync(testPost);
            await postRepository.AddAsync(testPost2);
            await postRepository.AddAsync(testPost3);
            await postRepository.AddAsync(testPost4);
            await postRepository.AddAsync(testPost5);
            await postRepository.AddAsync(testPost6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await postRepository.GetAsync(new ContentContainsSpecification<Post>("GetAsyncWithPaginationSkipNegative"),
                new PagingSpecification(-2, 3),
                new SortSpecification<Post>(
                    new OrderBySpecification<Post>(x => x.Content),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count() == 3);
            Assert.Contains(result, x => x.Id == testPost.Id &&
                                         x.Content == testPost.Content);
            Assert.Contains(result, x => x.Id == testPost2.Id &&
                                         x.Content == testPost2.Content);
            Assert.Contains(result, x => x.Id == testPost3.Id &&
                                         x.Content == testPost3.Content);
        }

        [Fact]
        public async void GetAsyncWithPaginationSkipOutOfRange()
        {
            // Arrange
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "PostGetAsyncWithPaginationSkipOutOfRange@email.com", Password = "1234", Username = "ComGetAWiPagSkipOfRa" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "ComGetAWiPagSkipOfRa" });
            var testPost = new Post()
            {
                Author = user.Entity,
                Name = "PostGetAsyncWithPaginationSkipOutOfRange", Category = category.Entity,
                Content = "PostGetAsyncWithPaginationSkipOutOfRange"
            };
            var testPost2 = new Post()
            {
                Author = user.Entity,
                Name = "PostGetAsyncWithPaginationSkipOutOfRange2", Category = category.Entity,
                Content = "PostGetAsyncWithPaginationSkipOutOfRange2"
            };
            var testPost3 = new Post()
            {
                Author = user.Entity,
                Name = "PostGetAsyncWithPaginationSkipOutOfRange3", Category = category.Entity,
                Content = "PostGetAsyncWithPaginationSkipOutOfRange3"
            };
            var testPost5 = new Post()
            {
                Author = user.Entity,
                Name = "PostGetAsyncWithPaginationSkipOutOfRange4", Category = category.Entity,
                Content = "PostGetAsyncWithPaginationSkipOutOfRange4"
            };
            var testPost6 = new Post()
            {
                Author = user.Entity,
                Name = "PostGetAsyncWithPaginationSkipOutOfRange5", Category = category.Entity,
                Content = "PostGetAsyncWithPaginationSkipOutOfRange5"
            };
            var testPost4 = new Post()
            {
                Author = user.Entity,
                Name = "PostGetAsyncWithPaginationSkipOutOfRange6", Category = category.Entity,
                Content = "PostGetAsyncWithPaginationSkipOutOfRange6"
            };
            await postRepository.AddAsync(testPost);
            await postRepository.AddAsync(testPost2);
            await postRepository.AddAsync(testPost3);
            await postRepository.AddAsync(testPost4);
            await postRepository.AddAsync(testPost5);
            await postRepository.AddAsync(testPost6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await postRepository.GetAsync(new ContentContainsSpecification<Post>("GetAsyncWithPaginationSkipOutOfRange"),
                new PagingSpecification(7, 3),
                new SortSpecification<Post>(
                    new OrderBySpecification<Post>(x => x.Content),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(!result.Any());
        }

        [Fact]
        public void Remove()
        {
            // Arrange
            var nbCategoriesAtBeginning = _fixture.Db.Posts.Count();
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var user = _fixture.Db.Users.Add(
                new User() { EmailAddress = "PostRemove@email.com", Password = "1234", Username = "PostRemove" });
            var category = _fixture.Db.Categories.Add(new Category() { Name = "CPostGetRemove" });
            var testPost = new Post()
            {
                Author = user.Entity,
                Name = "RemovePost", Category = category.Entity,
                Content = "Remove"
            };
            postRepository.Add(testPost);
            _fixture.UnitOfWork.Save();
            var nbCategoriesAfterAdded = _fixture.Db.Posts.Count();

            // Act
            postRepository.Remove(testPost);
            _fixture.UnitOfWork.Save();

            // Assert
            var nbCategoriesAfterRemoved = _fixture.Db.Posts.Count();
            Assert.True(nbCategoriesAtBeginning + 1 == nbCategoriesAfterAdded &&
                        nbCategoriesAfterRemoved == nbCategoriesAtBeginning);
        }

        [Fact]
        public void GetPost()
        {
            // Arrange
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);

            // Act
            var result = postRepository.Get(1);

            // Act & Assert
            Assert.True(result == _fixture.Db.Posts.Find(1));
        }

        [Fact]
        public void GetCategoryOutOfRange()
        {
            // Arrange
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);

            // Act & Assert
            Assert.Throws<IndexOutOfRangeException>(() => postRepository.Get(100));
        }

        [Fact]
        public void RemoveNull()
        {
            // Arrange
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => postRepository.Remove(null));
        }

        [Fact]
        public async void RemoveRangeAsyncNull()
        {
            // Arrange
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await postRepository.RemoveRangeAsync(null));
        }

        [Fact]
        public void RemoveRangeNull()
        {
            // Arrange
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => postRepository.RemoveRange(null));
        }

        [Fact]
        public void RemoveRange()
        {
            // Arrange
            var nbCategoriesAtBeginning = _fixture.Db.Posts.Count();
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var category = _fixture.Db.Categories.Add(new Category() { Name = "PostRemoveRange" });
            var user = _fixture.Db.Users.Add(
                new User() { EmailAddress = "PostRemoveRange@email.com", Password = "1234", Username = "PostGetRemoveRg" });
            var testPost = new Post()
            {
                Author = user.Entity,
                Name = "PostRemoveRange1", Category = category.Entity,
                Content = "PostRemoveRange1"
            };
            var testPost2 = new Post()
            {
                Author = user.Entity,
                Name = "PostRemoveRange2", Category = category.Entity,
                Content = "PostRemoveRange2"
            };
            postRepository.Add(testPost);
            postRepository.Add(testPost2);
            _fixture.UnitOfWork.Save();
            var nbCategoriesAfterAdded = _fixture.Db.Posts.Count();

            // Act
            postRepository.RemoveRange(new List<Post>() { testPost, testPost2 });
            _fixture.UnitOfWork.Save();

            // Assert
            var nbCategoriesAfterRemoved = _fixture.Db.Posts.Count();
            Assert.True(nbCategoriesAtBeginning + 2 == nbCategoriesAfterAdded &&
                        nbCategoriesAfterRemoved == nbCategoriesAtBeginning);
        }

        [Fact]
        public async void RemoveRangeAsync()
        {
            // Arrange
            var nbCategoriesAtBeginning = _fixture.Db.Posts.Count();
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var category = _fixture.Db.Categories.Add(new Category() { Name = "PostRemoveRange" });
            var user = _fixture.Db.Users.Add(
                new User() { EmailAddress = "PostRemoveRangeAsync@email.com", Password = "1234", Username = "PostRemoveRgAsync" });
            var testPost = new Post()
            {
                Author = user.Entity,
                Name = "PostRemoveRangeAsync1", Category = category.Entity,
                Content = "PostRemoveRangeAsync1"
            };
            var testPost2 = new Post()
            {
                Author = user.Entity,
                Name = "PostRemoveRangeAsync2", Category = category.Entity,
                Content = "PostRemoveRangeAsync2"
            };
            await postRepository.AddAsync(testPost);
            await postRepository.AddAsync(testPost2);
            _fixture.UnitOfWork.Save();
            var nbCategoriesAfterAdded = _fixture.Db.Posts.Count();

            // Act
            await postRepository.RemoveRangeAsync(new List<Post>() { testPost, testPost2 });
            _fixture.UnitOfWork.Save();

            // Assert
            var nbCategoriesAfterRemoved = _fixture.Db.Posts.Count();
            Assert.True(nbCategoriesAtBeginning + 2 == nbCategoriesAfterAdded &&
                        nbCategoriesAfterRemoved == nbCategoriesAtBeginning);
        }

        [Fact]
        public async void NameAlreadyExistsFalse()
        {
            // Arrange
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);

            // Act & Assert
            Assert.True(!await postRepository.NameAlreadyExists("NameAlreadyExistsFalse"));
        }

        [Fact]
        public async void NameAlreadyExistsNull()
        {
            // Arrange
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);

            // Act & Assert
            Assert.True(!await postRepository.NameAlreadyExists(null));
        }

        [Fact]
        public async void NameAlreadyExistsTrue()
        {
            // Arrange
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var category = _fixture.Db.Categories.Add(new Category() { Name = "NameAlreadyExistsTrue" });
            var user = _fixture.Db.Users.Add(
                new User() { EmailAddress = "NameAlreadyExistsTrue@email.com", Password = "1234", Username = "NameAlreadyExistsTrue" });
            var testPost = new Post()
            {
                Author = user.Entity,
                Name = "NameAlreadyExistsTrue",
                Category = category.Entity,
                Content = "NameAlreadyExistsTrue"
            };
            await postRepository.AddAsync(testPost);
            await _fixture.Db.SaveChangesAsync();

            // Act & Assert
            Assert.True(await postRepository.NameAlreadyExists(testPost.Name));
        }
    }
}
