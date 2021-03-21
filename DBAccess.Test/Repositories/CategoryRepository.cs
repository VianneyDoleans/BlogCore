using System;
using System.Linq;
using DbAccess.Data.POCO;
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
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            var testCategory = new Category() {Name = "testCategory"};
            await categoryRepository.AddAsync(testCategory);
            _fixture.UnitOfWork.Save();

            Assert.True(_fixture.Db.Categories.First(x => x.Name == testCategory.Name) != null);
        }

        [Fact]
        public async void AddNullCategoryAsync()
        {
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await categoryRepository.AddAsync(null));
        }

        [Fact]
        public async void GetCategoryAsync()
        {
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            var result = await categoryRepository.GetAsync(1);
            
            Assert.True(result == await _fixture.Db.Categories.FindAsync(1));
        }

        [Fact]
        public async void GetCategoryOutOfRangeAsync()
        {
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await categoryRepository.GetAsync(100));
        }

        [Fact]
        public async void GetAllAsync()
        {
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            var result = await categoryRepository.GetAllAsync();

            Assert.True(result.Count() == _fixture.Db.Categories.Count());
        }

        [Fact]
        public async void RemoveAsync()
        {
            var nbCategoriesAtBeginning = _fixture.Db.Categories.Count();
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);
            var testCategory = new Category()
            {
                Name = "TEST-42"
            };

            await categoryRepository.AddAsync(testCategory);
            _fixture.UnitOfWork.Save();
            var nbCategoriesAfterAdded = _fixture.Db.Categories.Count();
            await categoryRepository.RemoveAsync(testCategory);
            _fixture.UnitOfWork.Save();
            var nbCategoriesAfterRemoved = _fixture.Db.Categories.Count();

            Assert.True(nbCategoriesAtBeginning + 1 == nbCategoriesAfterAdded &&
                        nbCategoriesAfterRemoved == nbCategoriesAtBeginning);
        }

        [Fact]
        public async void RemoveNullAsync()
        {
            var categoryRepository = new DbAccess.Repositories.Category.CategoryRepository(_fixture.Db);

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await categoryRepository.RemoveAsync(null));
        }
    }
}
