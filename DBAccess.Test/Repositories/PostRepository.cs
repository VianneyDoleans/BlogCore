using System;
using System.Linq;
using DbAccess.Data.POCO;
using DbAccess.Specifications.FilterSpecifications.Filters;
using Xunit;

namespace DBAccess.Test.Repositories
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
               new User() { EmailAddress = "GetASpecBasicCo@email.com", Password = "1234", Username = "GetASpecBasicCo" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "GetASpecBasicCo" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "GetASpecBasicCo", Content = "GetASpecBasicCo", Category = category.Entity });
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
               new User() { EmailAddress = "GetAWiTwoSpecPst@email.com", Password = "1234", Username = "GetAWiTwoSpecPst" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "GetAWiTwoSpecPst" });
            _fixture.Db.SaveChanges();
            var testPost = new Post()
            {
                Author = user.Entity,
                Name = "GetAWiTwoSpecPst",
                Category = category.Entity,
                Content = "PostGetAsyncABSpecification"
            };
            var testPost2 = new Post()
            {
                Author = user.Entity,
                Name = "GetAWiTwoSpecPst2",
                Category = category.Entity,
                Content = "PostGetAsyncAUSpecification2"
            };
            var testPost3 = new Post()
            {
                Author = user.Entity,
                Name = "GetAWiTwoSpecPst3",
                Category = category.Entity,
                Content = "PostGetAsyncAKSpecification3"
            };
            var testPost4 = new Post()
            {
                Author = user.Entity,
                Name = "GetAWiTwoSpecPst4",
                Category = category.Entity,
                Content = "PostGetAsyncAKSpecification3164"
            };
            var testPost5 = new Post()
            {
                Author = user.Entity,
                Name = "GetAWiTwoSpecPst5",
                Category = category.Entity,
                Content = "PostGetAsyncAKSpecification32"
            };
            var testPost6 = new Post()
            {
                Author = user.Entity,
                Name = "GetAWiTwoSpecPst6",
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
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "PostTwoSortsAndTwoSpec", Content = "TwoSortsAndTwoSpecPost", Category = category.Entity });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, PostParent = post.Entity, Content = "TwwooGetAsyncWithTwoSortsPost" });
            var post2 = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user2.Entity, PostParent = post.Entity, Content = "GetAsyncWithTwoSorts2Post" });
            var post3 = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, PostParent = post.Entity, Content = "GetAsyncWithTwoSorts3TwwooPost" });
            var post4 = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user2.Entity, PostParent = post.Entity, Content = "AGetTwwooAsyncWithTwoSorts4Post" });
            var post5 = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user2.Entity, PostParent = post.Entity, Content = "GetAsyncTwwooWithTwoSorts5Post" });
            var post6 = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, PostParent = post.Entity, Content = "GetAsyncWithTwoSorts6Post" });
            var post7 = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, PostParent = post.Entity, Content = "TwwooGetAsyncWithTwoorts7Post" });
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
                new User() { EmailAddress = "PostGetAsyncWithAllArguments@email.com", Password = "1234", Username = "ComGetAWithAllArgs" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "ComGetAWithAllArgs" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "ComGetAWithAllArgs", Content = "ComGetAWithAllArgs", Category = category.Entity });
            var testPost = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "PostGetAsyncABSpecificationWithAllArguments"
            };
            var testPost2 = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "PostGetAsyncAUSpecification2WithAllArguments"
            };
            var testPost3 = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "PostGetAsyncAKSpecification3WithAllArguments"
            };
            var testPost4 = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "PostGetAsyncAKSpecification3WithAllArguments163"
            };
            var testPost5 = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "PostGetAsyncAKSpecification3WithAllArguments2"
            };
            var testPost6 = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
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
                new User() { EmailAddress = "PostGetAsyncWithPagination@email.com", Password = "1234", Username = "GetAsyncWithPagination" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "GetAsyncWithPagination" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "GetAsyncWithPagination", Content = "GetAsyncWithPagination", Category = category.Entity });
            var testPost = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "Post1GetAsyncWithPagination1"
            };
            var testPost2 = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "Post1GetAsyncWithPagination2"
            };
            var testPost3 = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "Post1GetAsyncWithPagination3"
            };
            var testPost4 = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "Post1GetAsyncWithPagination4"
            };
            var testPost5 = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "Post1GetAsyncWithPagination5"
            };
            var testPost6 = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
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
                new User() { EmailAddress = "CoGetAWithPagTaOfRg@email.com", Password = "1234", Username = "CoGetAWithPagTaOfRg" });
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "CoGetAWithPagTaOfRg" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "CoGetAWithPagTaOfRg", Content = "CoGetAWithPagTaOfRg", Category = category.Entity });
            var testPost = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "PostGetAsyncWithPaginationTakeOutOfRange1"
            };
            var testPost2 = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "PostGetAsyncWithPaginationTakeOutOfRange2"
            };
            var testPost3 = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "PostGetAsyncWithPaginationTakeOutOfRange3"
            };
            var testPost5 = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "PostGetAsyncWithPaginationTakeOutOfRange4"
            };
            var testPost6 = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "PostGetAsyncWithPaginationTakeOutOfRange5"
            };
            var testPost4 = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
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
            var category = await _fixture.Db.Categories.AddAsync(new Category() { Name = "ComGetAWithAllArgs" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "ComGetAWithAllArgs", Content = "ComGetAWithAllArgs", Category = category.Entity });
            var testPost = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "PostGetAsyncWithPaginationTakeNegative1"
            };
            var testPost2 = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "PostGetAsyncWithPaginationTakeNegative2"
            };
            var testPost3 = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "PostGetAsyncWithPaginationTakeNegative3"
            };
            var testPost5 = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "PostGetAsyncWithPaginationTakeNegative4"
            };
            var testPost6 = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "PostGetAsyncWithPaginationTakeNegative5"
            };
            var testPost4 = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
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
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "ComGetAWithPagSkipNega", Content = "ComGetAWithPagSkipNega", Category = category.Entity });
            var testPost = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "PostGetAsyncWithPaginationSkipNegative"
            };
            var testPost2 = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "PostGetAsyncWithPaginationSkipNegative2"
            };
            var testPost3 = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "PostGetAsyncWithPaginationSkipNegative3"
            };
            var testPost4 = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "PostGetAsyncWithPaginationSkipNegative4"
            };
            var testPost5 = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "PostGetAsyncWithPaginationSkipSkipNegative5"
            };
            var testPost6 = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
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
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Name = "ComGetAWiPagSkipOfRa", Content = "ComGetAWiPagSkipOfRa", Category = category.Entity });
            var testPost = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "PostGetAsyncWithPaginationSkipOutOfRange"
            };
            var testPost2 = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "PostGetAsyncWithPaginationSkipOutOfRange2"
            };
            var testPost3 = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "PostGetAsyncWithPaginationSkipOutOfRange3"
            };
            var testPost5 = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "PostGetAsyncWithPaginationSkipOutOfRange4"
            };
            var testPost6 = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "PostGetAsyncWithPaginationSkipOutOfRange5"
            };
            var testPost4 = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
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
            var post = _fixture.Db.Posts.Add(
                new Post() { Author = user.Entity, Name = "PostRemove", Content = "CommenRemove", Category = category.Entity });
            var testPost = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
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
            var post = _fixture.Db.Posts.Add(
                new Post() { Author = user.Entity, Name = "PostRemoveRange", Content = "PostRemoveRange", Category = category.Entity });
            var testPost = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "PostRemoveRange1"
            };
            var testPost2 = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
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
                new User() { EmailAddress = "ComRemoveRangeAsync@email.com", Password = "1234", Username = "ComRemoveRgAsync" });
            var post = _fixture.Db.Posts.Add(
                new Post() { Author = user.Entity, Name = "ComRemoveRgAsync", Content = "ComRemoveRgAsync", Category = category.Entity });
            var testPost = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
                Content = "PostRemoveRangeAsync1"
            };
            var testPost2 = new Post()
            {
                Author = user.Entity,
                PostParent = post.Entity,
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
    }
}
