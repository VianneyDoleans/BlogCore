﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlogCoreAPI.Models.DTOs.Tag;
using BlogCoreAPI.Models.Exceptions;
using BlogCoreAPI.Services.TagService;
using BlogCoreAPI.Validators.Tag;
using DBAccess.Exceptions;
using DBAccess.Repositories.Tag;
using FluentValidation;
using Xunit;

namespace BlogCoreAPI.Tests.ServicesTests
{
    public class TagServiceTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;
        private readonly ITagService _service;

        public TagServiceTests(DatabaseFixture databaseFixture)
        {
            _fixture = databaseFixture;
            var config = new MapperConfiguration(cfg => { cfg.AddProfile(databaseFixture.MapperProfile); });
            var mapper = config.CreateMapper();
            _service = new BlogCoreAPI.Services.TagService.TagService(new TagRepository(_fixture.Db),
                mapper, _fixture.UnitOfWork, new TagDtoValidator());
        }

        [Fact]
        public async Task AddTag()
        {
            // Arrange
            var tag = new AddTagDto() {Name = "AddTagTest22"};

            // Act
            var tagAdded = await _service.AddTag(tag);

            // Assert
            Assert.Contains(await _service.GetAllTags(), x => x.Id == tagAdded.Id &&
                                                              x.Name == tag.Name);
        }

        [Fact]
        public async Task AddTagWithAlreadyExistingName()
        {
            // Arrange
            var tag = new AddTagDto()
            {
                Name = "AddTagWithAlreadyExistingName",
            };
            await _service.AddTag(tag);

            // Act && Assert
            await Assert.ThrowsAsync<InvalidRequestException>(async () => await _service.AddTag(tag));
        }

        [Fact]
        public async Task AddTagWithTooLongName()
        {
            // Arrange
            var tag = new AddTagDto() {Name = "Ths is a long long long long long long long name !!"};

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.AddTag(tag));
        }

        [Fact]
        public async Task AddTagWithNullName()
        {
            // Arrange
            var tag = new AddTagDto();

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.AddTag(tag));
        }

        [Fact]
        public async Task AddTagWithEmptyName()
        {
            // Arrange
            var tag = new AddTagDto() {Name = "  "};

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.AddTag(tag));
        }

        [Fact]
        public async Task GetTagNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _service.GetTag(685479));
        }

        [Fact]
        public async Task DeleteTag()
        {
            // Arrange
            var tag = await _service.AddTag(new AddTagDto()
            {
                Name = "DeleteTag"
            });

            // Act
            await _service.DeleteTag(tag.Id);

            // Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _service.GetTag(tag.Id));
        }

        [Fact]
        public async Task DeleteTagNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _service.DeleteTag(175574));
        }

        [Fact]
        public async Task GetAllTags()
        {
            // Arrange
            var tagToAdd = new AddTagDto()
            {
                Name = "GetAllTags"
            };
            var tagToAdd2 = new AddTagDto()
            {
                Name = "GetAllTags2"
            };
            await _service.AddTag(tagToAdd);
            await _service.AddTag(tagToAdd2);

            // Act
            var tags = (await _service.GetAllTags()).ToArray();

            // Assert
            Assert.Contains(tags, x => x.Name == tagToAdd.Name);
            Assert.Contains(tags, x => x.Name == tagToAdd2.Name);
        }

        [Fact]
        public async Task GetTag()
        {
            // Arrange
            var tagToAdd = new AddTagDto()
            {
                Name = "GetTag"
            };

            // Act
            var tagDb = await _service.GetTag((await _service.AddTag(tagToAdd)).Id);

            // Assert
            Assert.True(tagDb.Name == tagToAdd.Name);
        }

        [Fact]
        public async Task UpdateTag()
        {
            // Arrange
            var tag = await _service.AddTag(new AddTagDto()
            {
                Name = "UpdateTag"
            });
            var tagToUpdate = new UpdateTagDto()
            {
                Id = tag.Id,
                Name = "UpdateTag2"
            };

            // Act
            await _service.UpdateTag(tagToUpdate);
            var tagUpdated = await _service.GetTag(tagToUpdate.Id);

            // Assert
            Assert.True(tagUpdated.Name == tagToUpdate.Name);
        }

        [Fact]
        public async Task UpdateTagInvalid()
        {
            // Arrange
            var tag = await _service.AddTag(new AddTagDto()
            {
                Name = "UpdateTagInvalid"
            });
            var tagToUpdate = new UpdateTagDto()
            {
                Id = tag.Id,
                Name = ""
            };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.UpdateTag(tagToUpdate));
        }

        [Fact]
        public async Task UpdateTagMissingName()
        {
            // Arrange
            var tag = await _service.AddTag(new AddTagDto()
            {
                Name = "UpTagMisName"
            });
            var tagToUpdate = new UpdateTagDto()
            {
                Id = tag.Id
            };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.UpdateTag(tagToUpdate));
        }

        [Fact]
        public async Task UpdateTagNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _service.UpdateTag(new UpdateTagDto()
                { Id = 164854, Name = "UpdateTagNotFound" }));
        }

        [Fact]
        public async Task UpdateTagWithSameExistingProperties()
        {
            // Arrange
            var tag = await _service.AddTag(new AddTagDto()
            {
                Name = "UpdateTagWithSameExistingProperty",
            });
            var tagToUpdate = new UpdateTagDto()
            {
                Id = tag.Id,
                Name = "UpdateTagWithSameExistingProperty",
            };

            // Act
            await _service.UpdateTag(tagToUpdate);
            var tagUpdated = await _service.GetTag(tagToUpdate.Id);

            // Assert
            Assert.True(tagUpdated.Name == tagToUpdate.Name);
        }

        [Fact]
        public async Task UpdateTagWithSomeUniqueExistingUniquePropertiesFromAnotherTag()
        {
            // Arrange
            await _service.AddTag(new AddTagDto()
            {
                Name = "UpdateTagWithSomeUExistingProperty",
            });
            var tag2 = await _service.AddTag(new AddTagDto()
            {
                Name = "UpdateTagWithSomeUExistingProperty2",
            });
            var tagToUpdate = new UpdateTagDto()
            {
                Id = tag2.Id,
                Name = "UpdateTagWithSomeUExistingProperty",
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidRequestException>(async () => await _service.UpdateTag(tagToUpdate));
        }

        [Fact]
        public async Task AddNullTag()
        {

            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.AddTag(null));
        }

        [Fact]
        public async Task UpdateNullTag()
        {

            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.UpdateTag(null));
        }
    }
}
