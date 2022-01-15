using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using MyBlogAPI.DTO;
using MyBlogAPI.FunctionalTests.Helpers;
using Xunit;

namespace MyBlogAPI.FunctionalTests.GenericTests
{
    public abstract class AGenericTests<TGet, TAdd, TUpdate>
        where TGet : ADto, new() 
        where TAdd : new()
        where TUpdate : ADto, new()
    {
        protected readonly HttpClient Client;
        protected abstract IEntityHelper<TGet, TAdd, TUpdate> Helper { get; set; }

        protected AGenericTests(TestWebApplicationFactory factory)
        {
            Client = factory.CreateClient();
        }

        [Fact]
        public async Task CanGetEntities()
        {
            // Arrange & Act
            var entities = await Helper.GetAll();

            // Assert
            Assert.True(entities.Any());
        }

        public abstract Task<TGet> AddRandomEntity();

        [Fact]
        public async Task CanAddEntity()
        {
            // Arrange & Act
            var entity = await AddRandomEntity();
            var entities = await Helper.GetAll();

            // Assert
            Assert.Contains(entities, p => p.Equals(entity));
        }

        [Fact]
        public async Task CanGetEntity()
        {
            // Arrange
            var entity = await AddRandomEntity();

            // Act
            var result = await Helper.GetById(entity.Id);

            // Assert
            Assert.True(result.Equals(entity));
        }

        [Fact]
        public async Task CanUpdateEntity()
        {
            // Arrange
            var entityAdded = await AddRandomEntity();

            // Act
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            var mapper = config.CreateMapper();
            await Helper.UpdateRandomEntity(mapper.Map<TUpdate>(entityAdded));

            // Assert
            Assert.False(Helper.Equals(await Helper.GetById(entityAdded.Id), entityAdded));
        }

        [Fact]
        public async Task CanDeleteEntity()
        {
            // Arrange
            var user = await AddRandomEntity();

            // Act
            await Helper.RemoveIdentity(user.Id);

            // Assert
            Assert.DoesNotContain(await Helper.GetAll(), p => p.Equals(user));
        }
    }
}
