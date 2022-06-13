using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBAccess.Data;
using DBAccess.Repositories.Post;
using DBAccess.Specifications;
using DBAccess.Specifications.FilterSpecifications.Filters;
using DBAccess.Specifications.SortSpecification;
using DBAccess.Tests.Builders;
using Xunit;

namespace DBAccess.Tests.Repositories
{
    public class CategoryRepository
    {
        private readonly DatabaseFixture _fixture;

        public CategoryRepository()
        {
            _fixture = new DatabaseFixture();
        }

        [Fact]
        public async Task AddCategoryAsync()
        {
            // Arrange
            var categoryRepository = new DBAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            var testCategory = new Category() {Name = "testAddCategoryAsync" };
            
            // Act
            await categoryRepository.AddAsync(testCategory);
            _fixture.UnitOfWork.Save();

            // Assert
            Assert.True(_fixture.Db.Categories.First(x => x.Name == testCategory.Name) != null);
        }

        [Fact]
        public async Task AddNullCategoryAsync()
        {
            // Arrange
            var categoryRepository = new DBAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await categoryRepository.AddAsync(null);
                await _fixture.Db.SaveChangesAsync();
            });
        }

        [Fact]
        public async Task GetCategoryAsync()
        {
            // Arrange
            var categoryRepository = new DBAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            var entity = _fixture.Db.Categories.Add(new Category() { Name = "GetCategoryAsync" });
            _fixture.UnitOfWork.Save();

            // Act
            var result = await categoryRepository.GetAsync(entity.Entity.Id);

            // Act & Assert
            Assert.True(result == await _fixture.Db.Categories.FindAsync(1));
        }

        [Fact]
        public async Task GetCategoryOutOfRangeAsync()
        {
            // Arrange
            var categoryRepository = new DBAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            // Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await categoryRepository.GetAsync(100));
        }

        [Fact]
        public async Task GetAllAsync()
        {
            // Arrange
            var categoryRepository = new DBAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            // Act
            var result = await categoryRepository.GetAllAsync();

            // Assert
            Assert.True(result.Count() == _fixture.Db.Categories.Count());
        }

        [Fact]
        public async Task RemoveAsync()
        {
            // Arrange
            var nbCategoriesAtBeginning = _fixture.Db.Categories.Count();
            var categoryRepository = new DBAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            var testCategory = new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).Build();
            var nbCategoriesAfterAdded = _fixture.Db.Categories.Count();

            // Act
            await categoryRepository.RemoveAsync(testCategory);
            _fixture.UnitOfWork.Save();

            // Assert
            var nbCategoriesAfterRemoved = _fixture.Db.Categories.Count();
            Assert.True(nbCategoriesAtBeginning + 1 == nbCategoriesAfterAdded &&
                        nbCategoriesAfterRemoved == nbCategoriesAtBeginning);
        }

        [Fact]
        public async Task RemoveNullAsync()
        {
            // Arrange
            var categoryRepository = new DBAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await categoryRepository.RemoveAsync(null));
        }

        // -----------------------------

        [Fact]
        public void AddCategory()
        {
            // Arrange
            var categoryRepository = new DBAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            var testCategory = new Category() { Name = "testAddCategory" };

            // Act
            categoryRepository.Add(testCategory);
            _fixture.UnitOfWork.Save();

            // Assert
            Assert.True(_fixture.Db.Categories.First(x => x.Name == testCategory.Name) != null);
        }

        [Fact]
        public void AddNullCategory()
        {
            // Arrange
            var categoryRepository = new DBAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                categoryRepository.Add(null);
                _fixture.Db.SaveChanges();
            });
        }

        [Fact]
        public void CountAll()
        {
            // Arrange
            var categoryRepository = new DBAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            // Act & Assert
            Assert.True(_fixture.Db.Categories.Count() == categoryRepository.CountAll());
        }

        [Fact]
        public async Task NameAlreadyExistsTrue()
        {
            // Arrange
            var categoryRepository = new DBAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            var testCategory = new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).Build();

            // Act & Assert
            Assert.True(await categoryRepository.NameAlreadyExists(testCategory.Name));
        }


        [Fact]
        public async Task CountAllAsync()
        {
            // Arrange
            var categoryRepository = new DBAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            // Act & Assert
            Assert.True(_fixture.Db.Categories.Count() == await categoryRepository.CountAllAsync());
        }


        [Fact]
        public void GetAll()
        {
            // Arrange
            var categoryRepository = new DBAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            // Act
            var result = categoryRepository.GetAll();

            // Assert
            Assert.True(result.Count() == _fixture.Db.Categories.Count());
        }

        [Fact]
        public async Task NameAlreadyExistsNull()
        {
            // Arrange
            var categoryRepository = new DBAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            // Act & Assert
            Assert.True(!await categoryRepository.NameAlreadyExists(null));
        }

        [Fact]
        public async Task GetAsyncSpecificationBasic()
        {
            // Arrange
            const string name = "GetAsyncSpecification2";
            var categoryRepository = new DBAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).Build();
            var testCategory2 = new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName(name).Build();
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).Build();
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).Build();

            // Act
            var result = await categoryRepository.GetAsync(new NameSpecification<Category>(name));

            // Assert
            Assert.True(result.First().Name == testCategory2.Name);
        }

        [Fact]
        public async Task GetAsyncWithTwoSpecifications()
        {
            // Arrange
            var categoryRepository = new DBAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncABSpecification").Build();
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncAUSpecification2").Build();
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncAKSpecification3").Build();
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncAKSpecification32").Build();
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncAWSpecification4").Build();
            var testCategory = new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncAKSpecification3164").Build();

            // Act
            var result = (await categoryRepository.GetAsync(new NameContainsSpecification<Category>("AK") & new NameContainsSpecification<Category>("164"))).ToList();

            // Assert
            Assert.True(result.Count == 1 && result.First().Name == testCategory.Name);
        }

        [Fact]
        public async Task GetAsyncWithTwoSortsAndTwoSpecificationsAndPagination()
        {
            // Arrange
            var categoryRepository = new DBAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            var testCategory = new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("TwwooGetAsyncWithTwoSorts").Build();
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncWithTwoSorts2").Build();
            var testCategory3 = new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncWithTwoSorts3Twwoo").Build();
            var testCategory4 = new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("AGetTwwooAsyncWithTwoSorts4").Build();
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncTwwooWithTwoSorts5").Build();
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncWithTwoSorts6").Build();
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("TwwooGetAsyncWithTwoorts7").Build();

            var userRepository = new DBAccess.Repositories.User.UserRepository(_fixture.Db, _fixture.UserManager);
            var user = new UserBuilder(userRepository, _fixture.UnitOfWork)
                .WithEmailAddress("TwoSortsAndTwoSpec@email.com")
                .WithUserName("TwoSortsAndTwoSpec")
                .Build();

            var postRepository = new PostRepository(_fixture.Db);
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithAuthor(user).WithCategory(testCategory).WithName("TwoSortsAndTwoSpec1").Build();
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithAuthor(user).WithCategory(testCategory).WithName("TwoSortsAndTwoSpec2").Build();
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithAuthor(user).WithCategory(testCategory3).WithName("TwoSortsAndTwoSpec3").Build();
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithAuthor(user).WithCategory(testCategory3).WithName("TwoSortsAndTwoSpec4").Build();
            new PostBuilder(postRepository, _fixture.UnitOfWork, _fixture.Db).WithAuthor(user).WithCategory(testCategory4).WithName("TwoSortsAndTwoSpec5").Build();

            // Act
            var result = (await categoryRepository.GetAsync(new NameContainsSpecification<Category>("WithTwoSorts")
                                                            & new NameContainsSpecification<Category>("Twwoo"), new PagingSpecification(0, 20),
                new SortSpecification<Category>(new OrderBySpecification<Category>(x => x.Posts.Count), SortingDirectionSpecification.Descending) &
                new SortSpecification<Category>(new OrderBySpecification<Category>(x => x.Name), SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count == 4 && result.First().Name == testCategory3.Name);
        }

        [Fact]
        public async Task GetAsyncWithNoArgument()
        {
            // Arrange
            var categoryRepository = new DBAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            // Act & Assert
            Assert.True((await categoryRepository.GetAsync()).ToList().Count == _fixture.Db.Categories.Count());
        }

        [Fact]
        public async Task GetAsyncWithAllArguments()
        {
            // Arrange
            var categoryRepository = new DBAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncABSpecificationWithAllArguments").Build();
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncAUSpecification2WithAllArguments").Build();
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncAKSpecification3WithAllArguments").Build();
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncAKSpecification3WithAllArguments163").Build();
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncAWSpecification4WithAllArguments").Build();
            var testCategory = new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncAKSpecification3WithAllArguments2").Build();

            // Act
            var result = (await categoryRepository.GetAsync(new NameContainsSpecification<Category>("AK") &
                new NameContainsSpecification<Category>("WithAllArguments"),
                new PagingSpecification(0, 2),
                new SortSpecification<Category>(
                    new OrderBySpecification<Category>(x => x.Name),
                    SortingDirectionSpecification.Descending))).ToList();

            // Assert
            Assert.True(result.Count == 2 && result.First().Name == testCategory.Name);
        }

        [Fact]
        public async Task GetAsyncWithPagination()
        {
            // Arrange
            var categoryRepository = new DBAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("1GetAsyncWithPagination1").Build();
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("1GetAsyncWithPagination2").Build();
            var testCategory3 = new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("1GetAsyncWithPagination3").Build();
            var testCategory4 = new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("1GetAsyncWithPagination4").Build();
            var testCategory5 = new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("1GetAsyncWithPagination5").Build();
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("1GetAsyncWithPagination6").Build();

            // Act
            var result = (await categoryRepository.GetAsync(new NameContainsSpecification<Category>("1GetAsyncWithPagination"),
                new PagingSpecification(2, 3),
                new SortSpecification<Category>(
                    new OrderBySpecification<Category>(x => x.Name),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count == 3);
            Assert.Contains(result, x => x.Id == testCategory3.Id &&
                                         x.Name == testCategory3.Name);
            Assert.Contains(result, x => x.Id == testCategory4.Id &&
                                         x.Name == testCategory4.Name);
            Assert.Contains(result, x => x.Id == testCategory5.Id &&
                                         x.Name == testCategory5.Name);
        }

        [Fact]
        public async Task GetAsyncWithPaginationTakeOutOfRange()
        {
            // Arrange
            var categoryRepository = new DBAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncWithPaginationTakeOutOfRange1").Build();
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncWithPaginationTakeOutOfRange2").Build();
            var testCategory3 = new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncWithPaginationTakeOutOfRange3").Build();
            var testCategory5 = new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncWithPaginationTakeOutOfRange4").Build();
            var testCategory6 = new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncWithPaginationTakeOutOfRange5").Build();
            var testCategory4 = new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncWithPaginationTakeOutOfRange6").Build();

            // Act
            var result = (await categoryRepository.GetAsync(new NameContainsSpecification<Category>("GetAsyncWithPaginationTakeOutOfRange"),
                new PagingSpecification(2, 22),
                new SortSpecification<Category>(
                    new OrderBySpecification<Category>(x => x.Name),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count == 4);
            Assert.Contains(result, x => x.Id == testCategory3.Id &&
                                         x.Name == testCategory3.Name);
            Assert.Contains(result, x => x.Id == testCategory4.Id &&
                                         x.Name == testCategory4.Name);
            Assert.Contains(result, x => x.Id == testCategory5.Id &&
                                         x.Name == testCategory5.Name);
            Assert.Contains(result, x => x.Id == testCategory6.Id &&
                                         x.Name == testCategory6.Name);
        }

        [Fact]
        public async Task GetAsyncWithPaginationTakeNegative()
        {
            // Arrange
            var categoryRepository = new DBAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncWithPaginationTakeNegative1").Build();
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncWithPaginationTakeNegative2").Build();
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncWithPaginationTakeNegative3").Build();
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncWithPaginationTakeNegative4").Build();
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncWithPaginationTakeNegative5").Build();
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncWithPaginationTakeNegative6").Build();

            // Act
            var result = (await categoryRepository.GetAsync(new NameContainsSpecification<Category>("GetAsyncWithPaginationTakeNegative"),
                new PagingSpecification(2, -2),
                new SortSpecification<Category>(
                    new OrderBySpecification<Category>(x => x.Name),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(!result.Any());
        }

        [Fact]
        public async Task GetAsyncWithPaginationSkipNegative()
        {
            // Arrange
            var categoryRepository = new DBAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            var testCategory = new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncWithPaginationSkipNegative").Build();
            var testCategory2 = new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncWithPaginationSkipNegative2").Build();
            var testCategory3 = new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncWithPaginationSkipNegative3").Build();
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncWithPaginationSkipNegative4").Build();
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncWithPaginationSkipSkipNegative5").Build();
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncWithPaginationSkipSkipNegative6").Build();

            // Act
            var result = (await categoryRepository.GetAsync(new NameContainsSpecification<Category>("GetAsyncWithPaginationSkipNegative"),
                new PagingSpecification(-2, 3),
                new SortSpecification<Category>(
                    new OrderBySpecification<Category>(x => x.Name),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count == 3);
            Assert.Contains(result, x => x.Id == testCategory.Id &&
                                         x.Name == testCategory.Name);
            Assert.Contains(result, x => x.Id == testCategory2.Id &&
                                         x.Name == testCategory2.Name);
            Assert.Contains(result, x => x.Id == testCategory3.Id &&
                                         x.Name == testCategory3.Name);
        }

        [Fact]
        public async Task GetAsyncWithPaginationSkipOutOfRange()
        {
            // Arrange
            var categoryRepository = new DBAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncWithPaginationSkipOutOfRange").Build();
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncWithPaginationSkipOutOfRange2").Build();
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncWithPaginationSkipOutOfRange3").Build();
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncWithPaginationSkipOutOfRange4").Build();
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncWithPaginationSkipOutOfRange5").Build();
            new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).WithName("GetAsyncWithPaginationSkipOutOfRange6").Build();

            // Act
            var result = (await categoryRepository.GetAsync(new NameContainsSpecification<Category>("GetAsyncWithPaginationSkipOutOfRange"),
                new PagingSpecification(7, 3),
                new SortSpecification<Category>(
                    new OrderBySpecification<Category>(x => x.Name),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(!result.Any());
        }

        [Fact]
        public void Remove()
        {
            // Arrange
            var nbCategoriesAtBeginning = _fixture.Db.Categories.Count();
            var categoryRepository = new DBAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            var testCategory = new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).Build();
            var nbCategoriesAfterAdded = _fixture.Db.Categories.Count();

            // Act
            categoryRepository.Remove(testCategory);
            _fixture.UnitOfWork.Save();

            // Assert
            var nbCategoriesAfterRemoved = _fixture.Db.Categories.Count();
            Assert.True(nbCategoriesAtBeginning + 1 == nbCategoriesAfterAdded &&
                        nbCategoriesAfterRemoved == nbCategoriesAtBeginning);
        }

        [Fact]
        public void GetCategory()
        {
            // Arrange
            var categoryRepository = new DBAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            var entity = new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).Build();

            // Act
            var result = categoryRepository.Get(entity.Id);

            // Act & Assert
            Assert.True(result == _fixture.Db.Categories.Find(entity.Id));
        }

        [Fact]
        public void GetCategoryOutOfRange()
        {
            // Arrange
            var categoryRepository = new DBAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            // Act & Assert
            Assert.Throws<IndexOutOfRangeException>(() => categoryRepository.Get(100));
        }

        [Fact]
        public void RemoveNull()
        {
            // Arrange
            var categoryRepository = new DBAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => categoryRepository.Remove(null));
        }

        [Fact]
        public async Task RemoveRangeAsyncNull()
        {
            // Arrange
            var categoryRepository = new DBAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await categoryRepository.RemoveRangeAsync(null));
        }

        [Fact]
        public void RemoveRangeNull()
        {
            // Arrange
            var categoryRepository = new DBAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => categoryRepository.RemoveRange(null));
        }

        [Fact]
        public void RemoveRange()
        {
            // Arrange
            var nbCategoriesAtBeginning = _fixture.Db.Categories.Count();
            var categoryRepository = new DBAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            var testCategory = new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).Build();
            var testCategory2 = new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).Build();
            var nbCategoriesAfterAdded = _fixture.Db.Categories.Count();

            // Act
            categoryRepository.RemoveRange(new List<Category>() { testCategory, testCategory2 });
            _fixture.UnitOfWork.Save();

            // Assert
            var nbCategoriesAfterRemoved = _fixture.Db.Categories.Count();
            Assert.True(nbCategoriesAtBeginning + 2 == nbCategoriesAfterAdded &&
                        nbCategoriesAfterRemoved == nbCategoriesAtBeginning);
        }

        [Fact]
        public async Task RemoveRangeAsync()
        {
            // Arrange
            var nbCategoriesAtBeginning = _fixture.Db.Categories.Count();
            var categoryRepository = new DBAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            var testCategory = new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).Build();
            var testCategory2 = new CategoryBuilder(categoryRepository, _fixture.UnitOfWork).Build();
            var nbCategoriesAfterAdded = _fixture.Db.Categories.Count();

            // Act
            await categoryRepository.RemoveRangeAsync(new List<Category>() { testCategory, testCategory2 });
            _fixture.UnitOfWork.Save();

            // Assert
            var nbCategoriesAfterRemoved = _fixture.Db.Categories.Count();
            Assert.True(nbCategoriesAtBeginning + 2 == nbCategoriesAfterAdded &&
                        nbCategoriesAfterRemoved == nbCategoriesAtBeginning);
        }

        [Fact]
        public async Task NameAlreadyExistsFalse()
        {
            // Arrange
            var categoryRepository = new DBAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            // Act & Assert
            Assert.True(!await categoryRepository.NameAlreadyExists("NameAlreadyExistsFalse"));
        }
    }
}
