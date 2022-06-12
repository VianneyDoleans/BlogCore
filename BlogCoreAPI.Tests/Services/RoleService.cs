using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlogCoreAPI.DTOs.Role;
using BlogCoreAPI.Services.RoleService;
using DBAccess.Data.POCO;
using DBAccess.Data.POCO.Permission;
using DBAccess.Repositories.Role;
using Xunit;

namespace BlogCoreAPI.Tests.Services
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
            _service = new BlogCoreAPI.Services.RoleService.RoleService(new RoleRepository(_fixture.Db, _fixture.RoleManager),
                mapper, _fixture.UnitOfWork);
        }

        [Fact]
        public async Task AddRole()
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
        public async Task AddRoleWithAlreadyExistingName()
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
        public async Task AddRoleWithTooLongName()
        {
            // Arrange
            var role = new AddRoleDto() { Name = "Ths is a long name !!" };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.AddRole(role));
        }

        [Fact]
        public async Task AddRoleWithNullName()
        {
            // Arrange
            var role = new AddRoleDto();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.AddRole(role));
        }

        [Fact]
        public async Task AddRoleWithEmptyName()
        {
            // Arrange
            var role = new AddRoleDto() { Name = "  " };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.AddRole(role));
        }

        [Fact]
        public async Task GetRoleNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.GetRole(685479));
        }

        [Fact]
        public async Task DeleteRole()
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
        public async Task DeleteRoleNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.DeleteRole(175574));
        }

        [Fact]
        public async Task GetAllRoles()
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
        public async Task GetRole()
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
        public async Task UpdateRole()
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
        public async Task UpdateRoleInvalid()
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
        public async Task UpdateRoleMissingName()
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
        public async Task UpdateRoleNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.UpdateRole(new UpdateRoleDto()
            { Id = 164854, Name = "UpdateRoleNotFound" }));
        }

        [Fact]
        public async Task UpdateRoleWithSomeUniqueExistingUniquePropertiesFromAnotherRole()
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
        public async Task UpdateRoleWithSameExistingProperties()
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
        public async Task AddNullRole()
        {

            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.AddRole(null));
        }

        [Fact]
        public async Task UpdateNullRole()
        {

            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.UpdateRole(null));
        }

        [Fact]
        public async Task AddPermissionAsync()
        {
            // Arrange
            var role = new Role() { Name = "AddPermissionAsync" };

            await _fixture.RoleManager.CreateAsync(role);
            var roleId = (await _fixture.RoleManager.FindByNameAsync(role.Name)).Id;
            var permission = new Permission()
            {
                PermissionAction = PermissionAction.CanCreate,
                PermissionTarget = PermissionTarget.Tag,
                PermissionRange = PermissionRange.All
            };

            // Act
            await _service.AddPermissionAsync(roleId, permission);

            // Assert
            Assert.Contains(await _service.GetPermissionsAsync(roleId), x => 
                x.PermissionAction.Id == (int)permission.PermissionAction && 
                x.PermissionRange.Id == (int)permission.PermissionRange && 
                x.PermissionTarget.Id == (int)permission.PermissionTarget);
        }

        [Fact]
        public async Task AddPermissionAsyncToRoleDoesNotExists()
        {
            // Arrange
            var role = new Role() { Name = " APeAsTRDNEx" };

            await _fixture.RoleManager.CreateAsync(role);
            var permission = new Permission()
            {
                PermissionAction = PermissionAction.CanCreate,
                PermissionTarget = PermissionTarget.Tag,
                PermissionRange = PermissionRange.All
            };

            // Act && Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.AddPermissionAsync(24162, permission));
        }

        [Fact]
        public async Task RemovePermissionAsync()
        {
            // Arrange
            var role = new Role() { Name = "RePeAsync" };

            await _fixture.RoleManager.CreateAsync(role);
            var roleId = (await _fixture.RoleManager.FindByNameAsync(role.Name)).Id;
            var permission = new Permission()
            {
                PermissionAction = PermissionAction.CanCreate,
                PermissionTarget = PermissionTarget.Tag,
                PermissionRange = PermissionRange.All
            };

            // Act
            await _service.AddPermissionAsync(roleId, permission);

            // Assert
            Assert.Contains(await _service.GetPermissionsAsync(roleId), x =>
                x.PermissionAction.Id == (int)permission.PermissionAction &&
                x.PermissionRange.Id == (int)permission.PermissionRange &&
                x.PermissionTarget.Id == (int)permission.PermissionTarget);
        }

        [Fact]
        public async Task RemovePermissionAsyncToRoleDoesNotExists()
        {
            // Arrange
            var permission = new Permission()
            {
                PermissionAction = PermissionAction.CanCreate,
                PermissionTarget = PermissionTarget.Tag,
                PermissionRange = PermissionRange.All
            };

            // Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.RemovePermissionAsync(254121, permission));
        }

        [Fact]
        public async Task GetPermissionsAsync()
        {
            // Arrange
            var role = new Role() { Name = "GePeAsync" };

            await _fixture.RoleManager.CreateAsync(role);
            var roleId = (await _fixture.RoleManager.FindByNameAsync(role.Name)).Id;
            var permission = new Permission()
            {
                PermissionAction = PermissionAction.CanCreate,
                PermissionTarget = PermissionTarget.Tag,
                PermissionRange = PermissionRange.All
            };
            await _service.AddPermissionAsync(roleId, permission);

            // Act
            var getPermissions = await _service.GetPermissionsAsync(roleId);

            // Assert
            Assert.Contains(getPermissions, x =>
                x.PermissionAction.Id == (int)permission.PermissionAction &&
                x.PermissionRange.Id == (int)permission.PermissionRange &&
                x.PermissionTarget.Id == (int)permission.PermissionTarget);
        }

        [Fact]
        public async Task GetPermissionsAsyncWithRoleDoesNotExist()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.GetPermissionsAsync(12312));
        }
    }
}
