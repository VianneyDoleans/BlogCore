using System;
using AutoMapper;
using DbAccess.Repositories.Role;
using MyBlogAPI.DTO.Role;
using MyBlogAPI.DTO.Role;
using MyBlogAPI.Services.RoleService;
using Xunit;

namespace MyBlogAPI.Test.Services
{
    public class RoleService : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;
        private readonly IRoleService _service;

        public RoleService(DatabaseFixture databaseFixture)
        {
            _fixture = databaseFixture;
            var config = new MapperConfiguration(cfg => { cfg.AddProfile(databaseFixture.MapperProfile); });
            var mapper = config.CreateMapper();
            _service = new MyBlogAPI.Services.RoleService.RoleService(new RoleRepository(_fixture.Db),
                mapper, _fixture.UnitOfWork);
        }

        [Fact]
        public async void AddRole()
        {
            // Arrange
            var role = new AddRoleDto() {Name = "AddRoleTest1732"};
            
            // Act
            var roleAdded = await _service.AddRole(role);

            // Assert
            Assert.Contains(await _service.GetAllRoles(), x => x.Id == roleAdded.Id &&
                                                              x.Name == role.Name);
        }

        [Fact]
        public async void AddRoleWithTooLongName()
        {
            // Arrange
            var role = new AddRoleDto() { Name = "Ths is a long name !!" };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.AddRole(role));
        }

        [Fact]
        public async void AddRoleWithNullName()
        {
            // Arrange
            var role = new AddRoleDto();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.AddRole(role));
        }

        [Fact]
        public async void AddRoleWithEmptyName()
        {
            // Arrange
            var role = new AddRoleDto() { Name = "  " };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.AddRole(role));
        }

        [Fact]
        public async void GetRoleNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.GetRole(685479));
        }
    }
}
