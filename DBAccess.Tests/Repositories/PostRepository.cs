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
            // Arrange
            var repository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var testPosts = new Post() { Name = "This is a test for post", Content = "a content" };

            // Act
            await repository.AddAsync(testPosts);
            _fixture.UnitOfWork.Save();

            // Assert
            Assert.True(_fixture.Db.Posts.First(x => x.Name == testPosts.Name) != null);
        }

        [Fact]
        public async void AddNullPostsAsync()
        {
            // Arrange
            var repository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.AddAsync(null));
        }

        [Fact]
        public async void GetPostsAsync()
        {
            // Arrange
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var testPost = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).Build();

            // Act
            var result = await postRepository.GetAsync(testPost.Id);

            // Assert
            Assert.True(result == await _fixture.Db.Posts.FindAsync(testPost.Id));
        }

        [Fact]
        public async void GetPostsOutOfRangeAsync()
        {
            // Arrange
            var repository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);

            // Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await repository.GetAsync(100));
        }

        [Fact]
        public async void GetAllAsync()
        {
            // Arrange
            var repository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var result = await repository.GetAllAsync();

            // Act & Assert
            Assert.True(result.Count() == _fixture.Db.Posts.Count());
        }

        [Fact]
        public async void RemoveAsync()
        {
            // Arrange
            var nbPostsAtBeginning = _fixture.Db.Posts.Count();
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var testPosts = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).Build();
            var nbPostsAfterAdded = _fixture.Db.Posts.Count();

            // Act
            await postRepository.RemoveAsync(testPosts);
            _fixture.UnitOfWork.Save();
            var nbPostsAfterRemoved = _fixture.Db.Posts.Count();

            // Assert
            Assert.True(nbPostsAtBeginning + 1 == nbPostsAfterAdded &&
                        nbPostsAfterRemoved == nbPostsAtBeginning);
        }

        [Fact]
        public async void RemoveNullAsync()
        {
            // Arrange
            var repository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await repository.RemoveAsync(null));
        }

        // --------
            
        [Fact]
        public void AddPost()
        {
            // Arrange
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            var user = new UserBuilder(userRepository, _fixture.UnitOfWork).Build();
            var category = new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).Build();
            var testPost = new Post() { Author = user, Name = "AddPost", Content = "AddPost", Category = category };

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

            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).Build();
            var testPost = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).Build();
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).Build();
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).Build();

            // Act
            var result = await postRepository.GetAsync(new IdSpecification<Post>(testPost.Id));

            // Assert
            Assert.True(result.First().Id == testPost.Id);
        }

        [Fact]
        public async void GetAsyncWithTwoSpecifications()
        {
            // Arrange
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);

            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncABSpecification").Build();
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncAUSpecification2").Build();
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncAKSpecification3").Build();
            var testPost = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncAKSpecification3164").Build();
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncAKSpecification32").Build();
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncAWSpecification6").Build();

            // Act
            var result = (await postRepository.GetAsync(new ContentContainsSpecification<Post>("AK") & new ContentContainsSpecification<Post>("164"))).ToList();

            // Assert
            Assert.True(result.Count() == 1 && result.First().Content == testPost.Content);
        }
        
        [Fact]
        public async void GetAsyncWithTwoSortsAndTwoSpecificationsAndPagination()
        {
            // Arrange
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);
            var userRepository = new DbAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);

            var user = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("TwoSortsAndTwoSpecPst").Build();
            var user2 = new UserBuilder(userRepository, _fixture.UnitOfWork).WithUserName("TwoSortsAndTwoSpecPst2").Build();

            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithAuthor(user).WithContent("TwwooGetAsyncWithTwoSortsPost").Build();
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithAuthor(user2).WithContent("GetAsyncWithTwoSorts2Post").Build();
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithAuthor(user).WithContent("GetAsyncWithTwoSorts3TwwooPost").Build();
            var testPost = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithAuthor(user2).WithContent("AGetTwwooAsyncWithTwoSorts4Post").Build();
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithAuthor(user2).WithContent("GetAsyncTwwooWithTwoSorts5Post").Build();
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithAuthor(user).WithContent("GetAsyncWithTwoSorts6Post").Build();
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithAuthor(user).WithContent("TwwooGetAsyncWithTwoorts7Post").Build();

            // Act
            var result = (await postRepository.GetAsync(new ContentContainsSpecification<Post>("WithTwoSorts")
                                                            & new ContentContainsSpecification<Post>("Twwoo"), new PagingSpecification(0, 20),
                new SortSpecification<Post>(new OrderBySpecification<Post>(x => x.Author.UserName), SortingDirectionSpecification.Descending) &
                new SortSpecification<Post>(new OrderBySpecification<Post>(x => x.Content), SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count() == 4 && result.First().Content == testPost.Content);
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
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncABSpecificationWithAllArguments").Build();
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncAUSpecification2WithAllArguments").Build();
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncAKSpecification3WithAllArguments").Build();
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncAKSpecification3WithAllArguments163").Build();
            var testPost = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncAKSpecification3WithAllArguments2").Build();
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncAWSpecification4WithAllArguments").Build();

            // Act
            var result = (await postRepository.GetAsync(new ContentContainsSpecification<Post>("AK") &
                new ContentContainsSpecification<Post>("WithAllArguments"),
                new PagingSpecification(0, 2),
                new SortSpecification<Post>(
                    new OrderBySpecification<Post>(x => x.Content),
                    SortingDirectionSpecification.Descending))).ToList();

            // Assert
            Assert.True(result.Count() == 2 && result.First().Content == testPost.Content);
        }

        [Fact]
        public async void GetAsyncWithPagination()
        {
            // Arrange
            var postRepository = new DbAccess.Repositories.Post.PostRepository(_fixture.Db);

            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("Post1GetAsyncWithPagination1").Build();
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("Post1GetAsyncWithPagination2").Build();
            var testPost3 = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("Post1GetAsyncWithPagination3").Build();
            var testPost4 = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("Post1GetAsyncWithPagination4").Build();
            var testPost5 = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("Post1GetAsyncWithPagination5").Build();
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("Post1GetAsyncWithPagination6").Build();

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
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncWithPaginationTakeOutOfRange1").Build();
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncWithPaginationTakeOutOfRange2").Build();
            var testPost3 = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncWithPaginationTakeOutOfRange3").Build();
            var testPost5 = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncWithPaginationTakeOutOfRange4").Build();
            var testPost6 = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncWithPaginationTakeOutOfRange5").Build();
            var testPost4 = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncWithPaginationTakeOutOfRange6").Build();

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
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncWithPaginationTakeNegative1").Build();
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncWithPaginationTakeNegative2").Build();
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncWithPaginationTakeNegative3").Build();
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncWithPaginationTakeNegative4").Build();
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncWithPaginationTakeNegative5").Build();
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncWithPaginationTakeNegative6").Build();

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
            var testPost = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncWithPaginationSkipNegative").Build();
            var testPost2 = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncWithPaginationSkipNegative2").Build();
            var testPost3 = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncWithPaginationSkipNegative3").Build();
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncWithPaginationSkipNegative4").Build();
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncWithPaginationSkipNegative5").Build();
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncWithPaginationSkipNegative6").Build();

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
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncWithPaginationSkipOutOfRange").Build();
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncWithPaginationSkipOutOfRange2").Build();
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncWithPaginationSkipOutOfRange3").Build();
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncWithPaginationSkipOutOfRange4").Build();
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncWithPaginationSkipOutOfRange5").Build();
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithContent("PostGetAsyncWithPaginationSkipOutOfRange6").Build();

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
            var testPost = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).Build();
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
            var testPost = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).Build();

            // Act
            var result = postRepository.Get(testPost.Id);

            // Act & Assert
            Assert.True(result == _fixture.Db.Posts.Find(testPost.Id));
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
            var testPost = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).Build();
            var testPost2 = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).Build();
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
            var testPost = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).Build();
            var testPost2 = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).Build();
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
            var testPost = new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).Build();

            // Act & Assert
            Assert.True(await postRepository.NameAlreadyExists(testPost.Name));
        }
    }
}
