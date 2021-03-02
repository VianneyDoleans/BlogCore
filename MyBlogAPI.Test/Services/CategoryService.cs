using System;
using AutoMapper;
using DbAccess.Repositories.Category;
using MyBlogAPI.DTO.Category;
using MyBlogAPI.Services.CategoryService;
using Xunit;

namespace MyBlogAPI.Test.Services
{
    public class CategoryService : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;
        private readonly ICategoryService _service;

        public CategoryService(DatabaseFixture databaseFixture)
        {
            _fixture = databaseFixture;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(databaseFixture.MapperProfile);
            });
            var mapper = config.CreateMapper();
            _service = new MyBlogAPI.Services.CategoryService.CategoryService(new CategoryRepository(_fixture.Db),
                mapper, _fixture.UnitOfWork);
        }

        [Fact]
        public async void AddCategory()
        {
            // Arrange
            var category = new AddCategoryDto() {Name = "AddCategory"};

            // Act
            var categoryAdded = await _service.AddCategory(category);

            // Assert
            Assert.Contains(await _service.GetAllCategories() , x => x.Id == categoryAdded.Id 
                                                                     && x.Name == category.Name);
        }

        [Fact]
        public async void UpdateCategory()
        {
            // Arrange
            var category = new AddCategoryDto() { Name = "UpCategory" };
            var categoryAdded = await _service.AddCategory(category);
            var categoryToUpdate = new UpdateCategoryDto() {Id = categoryAdded.Id, Name = "UpCategory Here"};

            // Act
            await _service.UpdateCategory(categoryToUpdate);

            // Assert
            var categoryUpdated = await _service.GetCategory(categoryToUpdate.Id);
            Assert.True(categoryUpdated.Name == categoryToUpdate.Name);
        }

        [Fact]
        public async void AddCategoryWithAlreadyExistingName()
        {
            // Arrange
            var category = new AddCategoryDto() { Name = " AddCategoryWiAlExName" };
            var categoryAdded = _service.AddCategory(category);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.AddCategory(category));
        }

        [Fact]
        public async void AddCategoryWithNullName()
        {
            // Arrange
            var category = new AddCategoryDto();

            // Act & Assert
           await Assert.ThrowsAsync<ArgumentException>(async () => await _service.AddCategory(category));
        }

        [Fact]
        public async void AddCategoryWithTooLongName()
        {
            // Arrange
            var category = new AddCategoryDto() {Name = "AAAAAAAAAAAAAAAAAAAAAAAAAAlongNameForTestingPurpose" };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.AddCategory(category));
        }

        [Fact]
        public async void GetCategoryNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.GetCategory(685479));
        }
    }
}
