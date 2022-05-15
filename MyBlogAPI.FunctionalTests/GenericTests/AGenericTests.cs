using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using MyBlogAPI.DTOs;
using MyBlogAPI.DTOs.User;
using MyBlogAPI.FunctionalTests.Helpers;
using Xunit;

namespace MyBlogAPI.FunctionalTests.GenericTests
{
    public abstract class AGenericTests<TGet, TAdd, TUpdate> : IAsyncLifetime
        where TGet : ADto, new() 
        where TAdd : new()
        where TUpdate : ADto, new()
    {
        protected readonly HttpClient Client;
        protected abstract IEntityHelper<TGet, TAdd, TUpdate> Helper { get; set; }
        protected readonly UserLoginDto Admin;

        protected AGenericTests(TestWebApplicationFactory factory)
        {
            Client = factory.CreateClient();
            Admin = factory.Admin;
        }

        protected async Task Login()
        {
            var accountHelper = new AccountHelper(Client);
            var token = await accountHelper.GetJwtLoginToken(Admin);
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{token}");
        }

        [Fact]
        public async Task CanGetEntities()
        {
            // Arrange
            await AddRandomEntity();

            // Act 
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
            var getEntityFromApi = await Helper.GetById(entityAdded.Id);
            Assert.False(Helper.Equals(getEntityFromApi, entityAdded));
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

        public async Task InitializeAsync()
        {
            await Login();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
