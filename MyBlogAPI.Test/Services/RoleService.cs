using System;
using System.Linq;
using AutoMapper;
using DbAccess.Repositories.Role;
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
        public async void AddRoleWithAlreadyExistingName()
        {
            // Arrange
            var role = new AddRoleDto()
            {
                Name = "AddRlWiAlExName",
            };
            await _service.AddRole(role);

            // Act && Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.AddRole(role));
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

        [Fact]
        public async void DeleteRole()
        {
            // Arrange
            var role = await _service.AddRole(new AddRoleDto()
            {
                Name = "DeleteRole"
            });

            // Act
            await _service.DeleteRole(role.Id);

            // Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.GetRole(role.Id));
        }

        [Fact]
        public async void DeleteRoleNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.DeleteRole(175574));
        }

        [Fact]
        public async void GetAllRoles()
        {
            // Arrange
            var roleToAdd = new AddRoleDto()
            {
                Name = "GetAllRoles"
            };
            var roleToAdd2 = new AddRoleDto()
            {
                Name = "GetAllRoles2"
            };
            await _service.AddRole(roleToAdd);
            await _service.AddRole(roleToAdd2);

            // Act
            var roles = (await _service.GetAllRoles()).ToArray();

            // Assert
            Assert.Contains(roles, x => x.Name == roleToAdd.Name);
            Assert.Contains(roles, x => x.Name == roleToAdd2.Name);
        }

        [Fact]
        public async void GetRole()
        {
            // Arrange
            var roleToAdd = new AddRoleDto()
            {
                Name = "GetRole"
            };

            // Act
            var roleDb = await _service.GetRole((await _service.AddRole(roleToAdd)).Id);

            // Assert
            Assert.True(roleDb.Name == roleToAdd.Name);
        }

        [Fact]
        public async void UpdateRole()
        {
            // Arrange
            var role = await _service.AddRole(new AddRoleDto()
            {
                Name = "UpdateRole"
            });
            var roleToUpdate = new UpdateRoleDto()
            {
                Id = role.Id,
                Name = "UpdateRole2"
            };

            // Act
            await _service.UpdateRole(roleToUpdate);
            var roleUpdated = await _service.GetRole(roleToUpdate.Id);

            // Assert
            Assert.True(roleUpdated.Name == roleToUpdate.Name);
        }

        [Fact]
        public async void UpdateRoleInvalid()
        {
            // Arrange
            var role = await _service.AddRole(new AddRoleDto()
            {
                Name = "UpdateRoleInvalid"
            });
            var roleToUpdate = new UpdateRoleDto()
            {
                Id = role.Id,
                Name = ""
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.UpdateRole(roleToUpdate));
        }

        [Fact]
        public async void UpdateRoleMissingName()
        {
            // Arrange
            var role = await _service.AddRole(new AddRoleDto()
            {
                Name = "UpRoleMisName"
            });
            var roleToUpdate = new UpdateRoleDto()
            {
                Id = role.Id,
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.UpdateRole(roleToUpdate));
        }

        [Fact]
        public async void UpdateRoleNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.UpdateRole(new UpdateRoleDto()
            { Id = 164854, Name = "UpdateRoleNotFound" }));
        }

        [Fact]
        public async void UpdateRoleWithSomeUniqueExistingUniquePropertiesFromAnotherRole()
        {
            // Arrange
            await _service.AddRole(new AddRoleDto()
            {
                Name = "UpRoleWiSoUExtProp",
            });
            var role2 = await _service.AddRole(new AddRoleDto()
            {
                Name = "UpRoleWiSoUExtProp2",
            });
            var roleToUpdate = new UpdateRoleDto()
            {
                Id = role2.Id,
                Name = "UpRoleWiSoUExtProp",
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.UpdateRole(roleToUpdate));
        }


        [Fact]
        public async void UpdateRoleWithSameExistingProperties()
        {
            // Arrange
            var role = await _service.AddRole(new AddRoleDto()
            {
                Name = "UpRoleWithSaExtProp",
            });
            var roleToUpdate = new UpdateRoleDto()
            {
                Id = role.Id,
                Name = "UpRoleWithSaExtProp",
            };

            // Act
            await _service.UpdateRole(roleToUpdate);
            var roleUpdated = await _service.GetRole(roleToUpdate.Id);

            // Assert
            Assert.True(roleUpdated.Name == roleToUpdate.Name);
        }

        [Fact]
        public async void AddNullRole()
        {

            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.AddRole(null));
        }

        [Fact]
        public async void UpdateNullRole()
        {

            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.UpdateRole(null));
        }


    }
}
