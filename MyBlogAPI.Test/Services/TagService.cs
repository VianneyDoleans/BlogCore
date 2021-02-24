using System;
using AutoMapper;
using DbAccess.Repositories.Tag;
using MyBlogAPI.DTO.Tag;
using MyBlogAPI.Services.TagService;
using Xunit;

namespace MyBlogAPI.Test.Services
{
    public class TagService : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;
        private readonly ITagService _service;

        public TagService(DatabaseFixture databaseFixture)
        {
            _fixture = databaseFixture;
            var config = new MapperConfiguration(cfg => { cfg.AddProfile(databaseFixture.MapperProfile); });
            var mapper = config.CreateMapper();
            _service = new MyBlogAPI.Services.TagService.TagService(new TagRepository(_fixture.Db),
                mapper, _fixture.UnitOfWork);
        }

        [Fact]
        public async void AddTag()
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
        public async void AddTagWithTooLongName()
        {
            // Arrange
            var tag = new AddTagDto() { Name = "Ths is a long long long long long long long name !!" };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.AddTag(tag));
        }

        [Fact]
        public async void AddTagWithNullName()
        {
            // Arrange
            var tag = new AddTagDto();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.AddTag(tag));
        }

        [Fact]
        public async void AddTagWithEmptyName()
        {
            // Arrange
            var tag = new AddTagDto() {Name = "  "};

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.AddTag(tag));
        }

        [Fact]
        public async void GetTagNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.GetTag(685479));
        }
    }
}
