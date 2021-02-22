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
            var config = new MapperConfiguration(cfg => { cfg.AddProfile(new AutoMapperProfile()); });
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
        public void AddRoleWithTooLongName()
        {
            // Arrange
            var role = new AddRoleDto() { Name = "Ths is a long name !!" };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _service.AddRole(role).Result);
        }

        [Fact]
        public void AddRoleWithNullName()
        {
            // Arrange
            var role = new AddRoleDto();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _service.AddRole(role).Result);
        }

        [Fact]
        public void AddRoleWithEmptyName()
        {
            // Arrange
            var role = new AddRoleDto() { Name = "  " };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _service.AddRole(role).Result);
        }

        [Fact]
        public void GetRoleNotFound()
        {
            // Arrange & Act & Assert
            Assert.Throws<IndexOutOfRangeException>(() => _service.GetRole(685479).Result);
        }
    }
}
