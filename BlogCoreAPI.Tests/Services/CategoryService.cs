using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlogCoreAPI.DTOs.Category;
using BlogCoreAPI.Services.CategoryService;
using BlogCoreAPI.Validators.Category;
using DBAccess.Data.POCO;
using DBAccess.Repositories.Category;
using DBAccess.Specifications.FilterSpecifications.Filters;
using DBAccess.Specifications.SortSpecification;
using FluentValidation;
using Xunit;

namespace BlogCoreAPI.Tests.Services
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
            _service = new BlogCoreAPI.Services.CategoryService.CategoryService(new CategoryRepository(_fixture.Db),
                mapper, _fixture.UnitOfWork, new CategoryDtoValidator());
        }

        [Fact]
        public async Task AddCategory()
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
        public async Task AddCategoryWithEmptyName()
        {
            // Arrange
            var category = new AddCategoryDto() { Name = "  " };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.AddCategory(category));
        }

        [Fact]
        public async Task UpdateCategory()
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
        public async Task AddCategoryWithAlreadyExistingName()
        {
            // Arrange
            var category = new AddCategoryDto() { Name = " AddCategoryWiAlExName" };
            await _service.AddCategory(category);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.AddCategory(category));
        }

        [Fact]
        public async Task AddCategoryWithNullName()
        {
            // Arrange
            var category = new AddCategoryDto();

            // Act & Assert
           await Assert.ThrowsAsync<ValidationException>(async () => await _service.AddCategory(category));
        }

        [Fact]
        public async Task AddCategoryWithTooLongName()
        {
            // Arrange
            var category = new AddCategoryDto() {Name = "AAAAAAAAAAAAAAAAAAAAAAAAAAlongNameForTestingPurpose" };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.AddCategory(category));
        }

        [Fact]
        public async Task GetCategoryNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.GetCategory(685479));
        }

        [Fact]
        public async Task DeleteCategory()
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
        public async Task DeleteCategoryNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.DeleteCategory(175574));
        }

        [Fact]
        public async Task GetAllCategories()
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
            var categories = (await _service.GetAllCategories()).ToArray();

            // Assert
            Assert.Contains(categories, x => x.Name == categoryToAdd.Name);
            Assert.Contains(categories, x => x.Name == categoryToAdd2.Name);
        }

        [Fact]
        public async Task GetCategories()
        {
            // Arrange
            var categoryToAdd = new AddCategoryDto()
            {
                Name = "ZGetCategories1"
            };
            var categoryToAdd2 = new AddCategoryDto()
            {
                Name = "BGetCategories"
            };
            var categoryToAdd3 = new AddCategoryDto()
            {
                Name = "BGetACategories"
            };
            var categoryToAdd4 = new AddCategoryDto()
            {
                Name = "EGetCategories"
            };
            await _service.AddCategory(categoryToAdd);
            await _service.AddCategory(categoryToAdd2);
            await _service.AddCategory(categoryToAdd3);
            await _service.AddCategory(categoryToAdd4);

            // Act
            var categories = (await _service.GetCategories(new NameContainsSpecification<Category>("BGetC"), null, 
                new SortSpecification<Category>(new OrderBySpecification<Category>(x => x.Name), 
                    SortingDirectionSpecification.Ascending)));

            // Assert
            Assert.True(categories.First().Name == categoryToAdd2.Name);
        }

        [Fact]
        public async Task GetCategory()
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
        public async Task UpdateCategoryInvalid()
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
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.UpdateCategory(categoryToUpdate));
        }

        [Fact]
        public async Task UpdateCategoryMissingName()
        {
            // Arrange
            var category = await _service.AddCategory(new AddCategoryDto()
            {
                Name = "UpCategoryMisName"
            });
            var categoryToUpdate = new UpdateCategoryDto()
            {
                Id = category.Id
            };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.UpdateCategory(categoryToUpdate));
        }

        [Fact]
        public async Task UpdateCategoryNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.UpdateCategory(new UpdateCategoryDto()
            { Id = 164854, Name = "UpdateCategoryNotFound" }));
        }

        [Fact]
        public async Task UpdateCategoryWithSameExistingProperties()
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
        public async Task UpdateCategoryWithSomeUniqueExistingPropertiesFromAnotherCategory()
        {
            // Arrange
            await _service.AddCategory(new AddCategoryDto()
            {
                Name = "UpCategoryWithSoUExtProp",
            });
            var category2 = await _service.AddCategory(new AddCategoryDto()
            {
                Name = "UpCategoryWithSoUExtProp2",
            });
            var categoryToUpdate = new UpdateCategoryDto()
            {
                Id = category2.Id,
                Name = "UpCategoryWithSoUExtProp",
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.UpdateCategory(categoryToUpdate));
        }

        [Fact]
        public async Task AddNullCategory()
        {

            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.AddCategory(null));
        }

        [Fact]
        public async Task UpdateNullCategory()
        {

            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.UpdateCategory(null));
        }
    }
}
