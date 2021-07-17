using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DbAccess.Data.POCO;
using DbAccess.Specifications;
using DbAccess.Specifications.FilterSpecifications;
using DbAccess.Specifications.FilterSpecifications.Filters;
using DbAccess.Specifications.SortSpecification;
using Xunit;

namespace DBAccess.Test.Repositories
{
    public class CategoryRepository
    {
        private readonly DatabaseFixture _fixture;

        public CategoryRepository()
        {
            _fixture = new DatabaseFixture();
        }

        [Fact]
        public async void AddCategoryAsync()
        {
            // Arrange
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            var testCategory = new Category() {Name = "testAddCategoryAsync" };
            
            // Act
            await categoryRepository.AddAsync(testCategory);
            _fixture.UnitOfWork.Save();

            // Assert
            Assert.True(_fixture.Db.Categories.First(x => x.Name == testCategory.Name) != null);
        }

        [Fact]
        public void AddCategory()
        {
            // Arrange
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            var testCategory = new Category() { Name = "testAddCategory" };

            // Act
            categoryRepository.Add(testCategory);
            _fixture.UnitOfWork.Save();

            // Assert
            Assert.True(_fixture.Db.Categories.First(x => x.Name == testCategory.Name) != null);
        }

        [Fact]
        public async void AddNullCategoryAsync()
        {
            // Arrange
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await categoryRepository.AddAsync(null);
                await _fixture.Db.SaveChangesAsync();
            });
        }

        [Fact]
        public void AddNullCategory()
        {
            // Arrange
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            { 
                categoryRepository.Add(null);
                _fixture.Db.SaveChanges();
            });
        }

        [Fact]
        public async void GetCategoryAsync()
        {
            // Arrange
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            // Act
            var result = await categoryRepository.GetAsync(1);

            // Act & Assert
            Assert.True(result == await _fixture.Db.Categories.FindAsync(1));
        }


        [Fact]
        public void GetCategory()
        {
            // Arrange
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            // Act
            var result = categoryRepository.Get(1);

            // Act & Assert
            Assert.True(result == _fixture.Db.Categories.Find(1));
        }

        [Fact]
        public async void GetCategoryOutOfRangeAsync()
        {
            // Arrange
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            // Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await categoryRepository.GetAsync(100));
        }

        [Fact]
        public void GetCategoryOutOfRange()
        {
            // Arrange
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            // Act & Assert
            Assert.Throws<IndexOutOfRangeException>( () => categoryRepository.Get(100));
        }

        [Fact]
        public async void GetAllAsync()
        {
            // Arrange
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            // Act
            var result = await categoryRepository.GetAllAsync();

            // Assert
            Assert.True(result.Count() == _fixture.Db.Categories.Count());
        }

        [Fact]
        public void GetAll()
        {
            // Arrange
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            // Act
            var result = categoryRepository.GetAll();

            // Assert
            Assert.True(result.Count() == _fixture.Db.Categories.Count());
        }

        [Fact]
        public async void RemoveAsync()
        {
            // Arrange
            var nbCategoriesAtBeginning = _fixture.Db.Categories.Count();
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            var testCategory = new Category()
            {
                Name = "RemoveAsync"
            };
            await categoryRepository.AddAsync(testCategory);
            _fixture.UnitOfWork.Save();
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
        public void Remove()
        {
            // Arrange
            var nbCategoriesAtBeginning = _fixture.Db.Categories.Count();
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            var testCategory = new Category()
            {
                Name = "Remove"
            };
            categoryRepository.Add(testCategory);
            _fixture.UnitOfWork.Save();
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
        public async void RemoveNullAsync()
        {
            // Arrange
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await categoryRepository.RemoveAsync(null));
        }

        [Fact]
        public void RemoveNull()
        {
            // Arrange
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => categoryRepository.Remove(null));
        }

        [Fact]
        public async void RemoveRangeAsyncNull()
        {
            // Arrange
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await categoryRepository.RemoveRangeAsync(null));
        }

        [Fact]
        public void RemoveRangeNull()
        {
            // Arrange
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => categoryRepository.RemoveRange(null));
        }

        [Fact]
        public void RemoveRange()
        {
            // Arrange
            var nbCategoriesAtBeginning = _fixture.Db.Categories.Count();
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            var testCategory = new Category()
            {
                Name = "RemoveRange1"
            };
            var testCategory2 = new Category()
            {
                Name = "RemoveRange2"
            };
            categoryRepository.Add(testCategory);
            categoryRepository.Add(testCategory2);
            _fixture.UnitOfWork.Save();
            var nbCategoriesAfterAdded = _fixture.Db.Categories.Count();

            // Act
            categoryRepository.RemoveRange(new List<Category>() {testCategory, testCategory2});
            _fixture.UnitOfWork.Save();
            
            // Assert
            var nbCategoriesAfterRemoved = _fixture.Db.Categories.Count();
            Assert.True(nbCategoriesAtBeginning + 2 == nbCategoriesAfterAdded &&
                        nbCategoriesAfterRemoved == nbCategoriesAtBeginning);
        }

        [Fact]
        public async void RemoveRangeAsync()
        {
            // Arrange
            var nbCategoriesAtBeginning = _fixture.Db.Categories.Count();
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            var testCategory = new Category()
            {
                Name = "RemoveRangeAsync1"
            };
            var testCategory2 = new Category()
            {
                Name = "RemoveRangeAsync2"
            };
            await categoryRepository.AddAsync(testCategory);
            await categoryRepository.AddAsync(testCategory2);
            _fixture.UnitOfWork.Save();
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
        public async void CountAllAsync()
        {
            // Arrange
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            // Act & Assert
            Assert.True(_fixture.Db.Categories.Count() == await categoryRepository.CountAllAsync());
        }

        [Fact]
        public void CountAll()
        {
            // Arrange
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            // Act & Assert
            Assert.True(_fixture.Db.Categories.Count() == categoryRepository.CountAll());
        }

        [Fact]
        public async void NameAlreadyExistsTrue()
        {
            // Arrange
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            var testCategory = new Category()
            {
                Name = "NameAlreadyExistsValid"
            };
            await categoryRepository.AddAsync(testCategory);
            await _fixture.Db.SaveChangesAsync();

            // Act & Assert
            Assert.True(await categoryRepository.NameAlreadyExists(testCategory.Name));
        }

        [Fact]
        public async void NameAlreadyExistsFalse()
        {
            // Arrange
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            // Act & Assert
            Assert.True(!await categoryRepository.NameAlreadyExists("NameAlreadyExistsFalse"));
        }

        [Fact]
        public async void NameAlreadyExistsNull()
        {
            // Arrange
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            // Act & Assert
            Assert.True(!await categoryRepository.NameAlreadyExists(null));
        }

        [Fact]
        public async void GetAsyncSpecificationBasic()
        {
            // Arrange
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            var testCategory = new Category()
            {
                Name = "GetAsyncSpecification"
            };
            var testCategory2 = new Category()
            {
                Name = "GetAsyncSpecification2"
            };
            var testCategory3 = new Category()
            {
                Name = "GetAsyncSpecification3"
            };
            var testCategory4 = new Category()
            {
                Name = "GetAsyncSpecification4"
            };
            categoryRepository.Add(testCategory);
            categoryRepository.Add(testCategory2);
            categoryRepository.Add(testCategory3);
            categoryRepository.Add(testCategory4);
            _fixture.UnitOfWork.Save();

            // Act
            var result = await categoryRepository.GetAsync(new NameSpecification<Category>("GetAsyncSpecification2"));

            // Assert
            Assert.True(result.First().Name == testCategory2.Name);
        }

        [Fact]
        public async void GetAsyncWithTwoSpecifications()
        {
            // Arrange
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            var testCategory = new Category()
            {
                Name = "GetAsyncABSpecification"
            };
            var testCategory2 = new Category()
            {
                Name = "GetAsyncAUSpecification2"
            };
            var testCategory3 = new Category()
            {
                Name = "GetAsyncAKSpecification3"
            };
            var testCategory5 = new Category()
            {
                Name = "GetAsyncAKSpecification3164"
            };
            var testCategory6 = new Category()
            {
                Name = "GetAsyncAKSpecification32"
            };
            var testCategory4 = new Category()
            {
                Name = "GetAsyncAWSpecification4"
            };
            await categoryRepository.AddAsync(testCategory);
            await categoryRepository.AddAsync(testCategory2);
            await categoryRepository.AddAsync(testCategory3);
            await categoryRepository.AddAsync(testCategory4);
            await categoryRepository.AddAsync(testCategory5);
            await categoryRepository.AddAsync(testCategory6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await categoryRepository.GetAsync(new NameContainsSpecification<Category>("AK") & new NameContainsSpecification<Category>("164"))).ToList();

            // Assert
            Assert.True(result.Count() == 1 && result.First().Name == testCategory5.Name);
        }

        [Fact]
        public async void GetAsyncWithTwoSortsAndTwoSpecificationsAndPagination()
        {
            // Arrange
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            var testCategory = new Category()
            {
                Name = "TwwooGetAsyncWithTwoSorts",
            };
            var testCategory2 = new Category()
            {
                Name = "GetAsyncWithTwoSorts2"
            };
            var testCategory3 = new Category()
            {
                Name = "GetAsyncWithTwoSorts3Twwoo"
            };
            var testCategory4 = new Category()
            {
                Name = "AGetTwwooAsyncWithTwoSorts4"
            };
            var testCategory5 = new Category()
            {
                Name = "GetAsyncTwwooWithTwoSorts5"
            };
            var testCategory6 = new Category()
            {
                Name = "GetAsyncWithTwoSorts6"
            };
            var testCategory7 = new Category()
            {
                Name = "TwwooGetAsyncWithTwoorts7"
            };
            await categoryRepository.AddAsync(testCategory);
            await categoryRepository.AddAsync(testCategory2);
            await categoryRepository.AddAsync(testCategory3);
            await categoryRepository.AddAsync(testCategory4);
            await categoryRepository.AddAsync(testCategory5);
            await categoryRepository.AddAsync(testCategory6);
            await categoryRepository.AddAsync(testCategory7);

            var user = await _fixture.Db.Users.AddAsync(
                new User() { EmailAddress = "TwoSortsAndTwoSpec@email.com", Password = "1234", Username = "TwoSortsAndTwoSpec" });
            var post = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = testCategory, Content = "new post", Name = "TwoSortsAndTwoSpec1" });
            var post2 = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = testCategory, Content = "new post", Name = "TwoSortsAndTwoSpec2" });
            var post3 = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = testCategory3, Content = "new post", Name = "TwoSortsAndTwoSpec3" });
            var post4 = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = testCategory3, Content = "new post", Name = "TwoSortsAndTwoSpec4" });
            var post5 = await _fixture.Db.Posts.AddAsync(
                new Post() { Author = user.Entity, Category = testCategory4, Content = "new post", Name = "TwoSortsAndTwoSpec5" });

            _fixture.UnitOfWork.Save();

            // Act
            var result = (await categoryRepository.GetAsync(new NameContainsSpecification<Category>("WithTwoSorts") 
                                                            & new NameContainsSpecification<Category>("Twwoo"), new PagingSpecification(0, 20),
                new SortSpecification<Category>(new OrderBySpecification<Category>(x => x.Posts.Count), SortingDirectionSpecification.Descending) &
                new SortSpecification<Category>(new OrderBySpecification<Category>(x => x.Name), SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count() == 4 && result.First().Name == testCategory3.Name);
        }

        [Fact]
        public async void GetAsyncWithNoArgument()
        {
            // Arrange
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            // Act & Assert
            Assert.True((await categoryRepository.GetAsync()).ToList().Count() == _fixture.Db.Categories.Count());
        }

        [Fact]
        public async void GetAsyncWithAllArguments()
        {
            // Arrange
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            var testCategory = new Category()
            {
                Name = "GetAsyncABSpecificationWithAllArguments"
            };
            var testCategory2 = new Category()
            {
                Name = "GetAsyncAUSpecification2WithAllArguments"
            };
            var testCategory3 = new Category()
            {
                Name = "GetAsyncAKSpecification3WithAllArguments"
            };
            var testCategory4 = new Category()
            {
                Name = "GetAsyncAKSpecification3WithAllArguments163"
            };
            var testCategory5 = new Category()
            {
                Name = "GetAsyncAKSpecification3WithAllArguments2"
            };
            var testCategory6 = new Category()
            {
                Name = "GetAsyncAWSpecification4WithAllArguments"
            };
            await categoryRepository.AddAsync(testCategory);
            await categoryRepository.AddAsync(testCategory2);
            await categoryRepository.AddAsync(testCategory3);
            await categoryRepository.AddAsync(testCategory4);
            await categoryRepository.AddAsync(testCategory5);
            await categoryRepository.AddAsync(testCategory6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await categoryRepository.GetAsync(new NameContainsSpecification<Category>("AK") &
                new NameContainsSpecification<Category>("WithAllArguments"),
                new PagingSpecification(0, 2),
                new SortSpecification<Category>(
                    new OrderBySpecification<Category>(x => x.Name), 
                    SortingDirectionSpecification.Descending))).ToList();

            // Assert
            Assert.True(result.Count() == 2 && result.First().Name == testCategory5.Name);
        }

        [Fact]
        public async void GetAsyncWithPagination()
        {
            // Arrange
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            var testCategory = new Category()
            {
                Name = "1GetAsyncWithPagination1"
            };
            var testCategory2 = new Category()
            {
                Name = "1GetAsyncWithPagination2"
            };
            var testCategory3 = new Category()
            {
                Name = "1GetAsyncWithPagination3"
            };
            var testCategory4 = new Category()
            {
                Name = "1GetAsyncWithPagination4"
            };
            var testCategory5 = new Category()
            {
                Name = "1GetAsyncWithPagination5"
            };
            var testCategory6 = new Category()
            {
                Name = "1GetAsyncWithPagination6"
            };
            await categoryRepository.AddAsync(testCategory);
            await categoryRepository.AddAsync(testCategory2);
            await categoryRepository.AddAsync(testCategory3);
            await categoryRepository.AddAsync(testCategory4);
            await categoryRepository.AddAsync(testCategory5);
            await categoryRepository.AddAsync(testCategory6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await categoryRepository.GetAsync(new NameContainsSpecification<Category>("1GetAsyncWithPagination"),
                new PagingSpecification(2, 3),
                new SortSpecification<Category>(
                    new OrderBySpecification<Category>(x => x.Name),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count() == 3);
            Assert.Contains(result, x => x.Id == testCategory3.Id &&
                                         x.Name == testCategory3.Name);
            Assert.Contains(result, x => x.Id == testCategory4.Id &&
                                         x.Name == testCategory4.Name);
            Assert.Contains(result, x => x.Id == testCategory5.Id &&
                                         x.Name == testCategory5.Name);
        }

        [Fact]
        public async void GetAsyncWithPaginationTakeOutOfRange()
        {
            // Arrange
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            var testCategory = new Category()
            {
                Name = "GetAsyncWithPaginationTakeOutOfRange1"
            };
            var testCategory2 = new Category()
            {
                Name = "GetAsyncWithPaginationTakeOutOfRange2"
            };
            var testCategory3 = new Category()
            {
                Name = "GetAsyncWithPaginationTakeOutOfRange3"
            };
            var testCategory5 = new Category()
            {
                Name = "GetAsyncWithPaginationTakeOutOfRange4"
            };
            var testCategory6 = new Category()
            {
                Name = "GetAsyncWithPaginationTakeOutOfRange5"
            };
            var testCategory4 = new Category()
            {
                Name = "GetAsyncWithPaginationTakeOutOfRange6"
            };
            await categoryRepository.AddAsync(testCategory);
            await categoryRepository.AddAsync(testCategory2);
            await categoryRepository.AddAsync(testCategory3);
            await categoryRepository.AddAsync(testCategory4);
            await categoryRepository.AddAsync(testCategory5);
            await categoryRepository.AddAsync(testCategory6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await categoryRepository.GetAsync(new NameContainsSpecification<Category>("GetAsyncWithPaginationTakeOutOfRange"),
                new PagingSpecification(2, 22),
                new SortSpecification<Category>(
                    new OrderBySpecification<Category>(x => x.Name),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count() == 4);
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
        public async void GetAsyncWithPaginationTakeNegative()
        {
            // Arrange
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            var testCategory = new Category()
            {
                Name = "GetAsyncWithPaginationTakeNegative1"
            };
            var testCategory2 = new Category()
            {
                Name = "GetAsyncWithPaginationTakeNegative2"
            };
            var testCategory3 = new Category()
            {
                Name = "GetAsyncWithPaginationTakeNegative3"
            };
            var testCategory5 = new Category()
            {
                Name = "GetAsyncWithPaginationTakeNegative4"
            };
            var testCategory6 = new Category()
            {
                Name = "GetAsyncWithPaginationTakeNegative5"
            };
            var testCategory4 = new Category()
            {
                Name = "GetAsyncWithPaginationTakeNegative6"
            };
            await categoryRepository.AddAsync(testCategory);
            await categoryRepository.AddAsync(testCategory2);
            await categoryRepository.AddAsync(testCategory3);
            await categoryRepository.AddAsync(testCategory4);
            await categoryRepository.AddAsync(testCategory5);
            await categoryRepository.AddAsync(testCategory6);
            _fixture.UnitOfWork.Save();

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
        public async void GetAsyncWithPaginationSkipNegative()
        {
            // Arrange
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            var testCategory = new Category()
            {
                Name = "GetAsyncWithPaginationSkipNegative"
            };
            var testCategory2 = new Category()
            {
                Name = "GetAsyncWithPaginationSkipNegative2"
            };
            var testCategory3 = new Category()
            {
                Name = "GetAsyncWithPaginationSkipNegative3"
            };
            var testCategory4 = new Category()
            {
                Name = "GetAsyncWithPaginationSkipNegative4"
            };
            var testCategory5 = new Category()
            {
                Name = "GetAsyncWithPaginationSkipSkipNegative5"
            };
            var testCategory6 = new Category()
            {
                Name = "GetAsyncWithPaginationSkipSkipNegative6"
            };
            await categoryRepository.AddAsync(testCategory);
            await categoryRepository.AddAsync(testCategory2);
            await categoryRepository.AddAsync(testCategory3);
            await categoryRepository.AddAsync(testCategory4);
            await categoryRepository.AddAsync(testCategory5);
            await categoryRepository.AddAsync(testCategory6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await categoryRepository.GetAsync(new NameContainsSpecification<Category>("GetAsyncWithPaginationSkipNegative"),
                new PagingSpecification(-2, 3),
                new SortSpecification<Category>(
                    new OrderBySpecification<Category>(x => x.Name),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(result.Count() == 3);
            Assert.Contains(result, x => x.Id == testCategory.Id &&
                                         x.Name == testCategory.Name);
            Assert.Contains(result, x => x.Id == testCategory2.Id &&
                                         x.Name == testCategory2.Name);
            Assert.Contains(result, x => x.Id == testCategory3.Id &&
                                         x.Name == testCategory3.Name);
        }

        [Fact]
        public async void GetAsyncWithPaginationSkipOutOfRange()
        {
            // Arrange
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            var testCategory = new Category()
            {
                Name = "GetAsyncWithPaginationSkipOutOfRange"
            };
            var testCategory2 = new Category()
            {
                Name = "GetAsyncWithPaginationSkipOutOfRange2"
            };
            var testCategory3 = new Category()
            {
                Name = "GetAsyncWithPaginationSkipOutOfRange3"
            };
            var testCategory5 = new Category()
            {
                Name = "GetAsyncWithPaginationSkipOutOfRange4"
            };
            var testCategory6 = new Category()
            {
                Name = "GetAsyncWithPaginationSkipOutOfRange5"
            };
            var testCategory4 = new Category()
            {
                Name = "GetAsyncWithPaginationSkipOutOfRange6"
            };
            await categoryRepository.AddAsync(testCategory);
            await categoryRepository.AddAsync(testCategory2);
            await categoryRepository.AddAsync(testCategory3);
            await categoryRepository.AddAsync(testCategory4);
            await categoryRepository.AddAsync(testCategory5);
            await categoryRepository.AddAsync(testCategory6);
            _fixture.UnitOfWork.Save();

            // Act
            var result = (await categoryRepository.GetAsync(new NameContainsSpecification<Category>("GetAsyncWithPaginationSkipOutOfRange"),
                new PagingSpecification(7, 3),
                new SortSpecification<Category>(
                    new OrderBySpecification<Category>(x => x.Name),
                    SortingDirectionSpecification.Ascending))).ToList();

            // Assert
            Assert.True(!result.Any());
        }
    }
}
