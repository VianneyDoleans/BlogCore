using System;
using System.Linq;
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
        public async void AddCategoryWithEmptyName()
        {
            // Arrange
            var category = new AddCategoryDto() { Name = "  " };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.AddCategory(category));
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
            await _service.AddCategory(category);

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

        [Fact]
        public async void DeleteCategory()
        {
            // Arrange
            var category = await _service.AddCategory(new AddCategoryDto()
            {
                Name = "DeleteCategory"
            });

            // Act
            await _service.DeleteCategory(category.Id);

            // Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.GetCategory(category.Id));
        }

        [Fact]
        public async void DeleteCategoryNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.DeleteCategory(175574));
        }

        [Fact]
        public async void GetAllCategories()
        {
            // Arrange
            var categoryToAdd = new AddCategoryDto()
            {
                Name = "GetAllCategorys"
            };
            var categoryToAdd2 = new AddCategoryDto()
            {
                Name = "GetAllCategorys2"
            };
            await _service.AddCategory(categoryToAdd);
            await _service.AddCategory(categoryToAdd2);

            // Act
            var categorys = (await _service.GetAllCategories()).ToArray();

            // Assert
            Assert.Contains(categorys, x => x.Name == categoryToAdd.Name);
            Assert.Contains(categorys, x => x.Name == categoryToAdd2.Name);
        }

        [Fact]
        public async void GetCategory()
        {
            // Arrange
            var categoryToAdd = new AddCategoryDto()
            {
                Name = "GetCategory"
            };

            // Act
            var categoryDb = await _service.GetCategory((await _service.AddCategory(categoryToAdd)).Id);

            // Assert
            Assert.True(categoryDb.Name == categoryToAdd.Name);
        }

        [Fact]
        public async void UpdateCategoryInvalid()
        {
            // Arrange
            var category = await _service.AddCategory(new AddCategoryDto()
            {
                Name = "UpdateCategoryInvalid"
            });
            var categoryToUpdate = new UpdateCategoryDto()
            {
                Id = category.Id,
                Name = ""
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.UpdateCategory(categoryToUpdate));
        }

        [Fact]
        public async void UpdateCategoryNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.UpdateCategory(new UpdateCategoryDto()
            { Id = 164854, Name = "UpdateCategoryNotFound" }));
        }

        [Fact]
        public async void UpdateCategoryWithSameExistingProperties()
        {
            // Arrange
            var category = await _service.AddCategory(new AddCategoryDto()
            {
                Name = "UpCategoryWithSaExtProp",
            });
            var categoryToUpdate = new UpdateCategoryDto()
            {
                Id = category.Id,
                Name = "UpCategoryWithSaExtProp",
            };

            // Act
            await _service.UpdateCategory(categoryToUpdate);
            var categoryUpdated = await _service.GetCategory(categoryToUpdate.Id);

            // Assert
            Assert.True(categoryUpdated.Name == categoryToUpdate.Name);
        }

        [Fact]
        public async void AddNullCategory()
        {

            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.AddCategory(null));
        }

        [Fact]
        public async void UpdateNullCategory()
        {

            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.UpdateCategory(null));
        }
    }
}
