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
            var category = new AddCategoryDto() {Name = "ANewName13"};

            // Act
            var categoryAdded = await _service.AddCategory(category);

            // Assert
            Assert.Contains(await _service.GetAllCategories() , x => x.Id == categoryAdded.Id 
                                                                     && x.Name == category.Name);
        }

        [Fact]
        public void AddCategoryWithAlreadyExistingName()
        {
            // Arrange
            var category = new AddCategoryDto() { Name = "ANewName134" };
            var categoryAdded = _service.AddCategory(category);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _service.AddCategory(category).Result);
        }

        [Fact]
        public void AddCategoryWithNullName()
        {
            // Arrange
            var category = new AddCategoryDto();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.AddCategory(category).Result);
        }

        [Fact]
        public void AddCategoryWithTooLongName()
        {
            // Arrange
            var category = new AddCategoryDto() {Name = "AAAAAAAAAAAAAAAAAAAAAAAAAAlongNameForTestingPurpose" };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _service.AddCategory(category).Result);
        }

        [Fact]
        public void GetCategoryNotFound()
        {
            // Arrange & Act & Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.GetCategory(685479).Result);
        }
    }
}
