using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DbAccess.Data.POCO;
using DbAccess.Data.POCO.JoiningEntity;
using DbAccess.Repositories.User;
using MyBlogAPI.DTO.User;
using MyBlogAPI.Services.UserService;
using Xunit;

namespace MyBlogAPI.Tests.Services
{
    public class UserService : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;
        private readonly IUserService _service;

        public UserService(DatabaseFixture databaseFixture)
        {
            _fixture = databaseFixture;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(databaseFixture.MapperProfile);
            });
            var mapper = config.CreateMapper();
            _service = new MyBlogAPI.Services.UserService.UserService(new UserRepository(_fixture.Db, _fixture.UserManager), 
                mapper, _fixture.UnitOfWork);
        }

        [Fact]
        public async Task AddUser()
        {
            // Arrange
            var userToAdd = new AddUserDto()
            {
                Email = "AddUser@newEmail.com",
                Password = "16453aA-007",
                UserName = "AddUser1"
            };

            // Act
            var user = await _service.AddUser(userToAdd);

            // Assert
            Assert.Contains(_fixture.Db.Users, x => x.Id == user.Id &&
                                                    x.Email == user.Email &&
                                                    x.UserDescription == user.UserDescription &&
                                                    x.UserName == user.UserName);
        }

        [Fact]
        public async Task AddNullUser()
        {

            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.AddUser(null));
        }

        [Fact]
        public async Task UpdateNullUser()
        {

            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.UpdateUser(null));
        }

        [Fact]
        public async Task AddUserWithAnAlreadyExistingEmailAddress()
        {
            // Arrange
            var userToAdd = new AddUserDto()
            {
                Email = "AddUserAlreadyEmail@newEmail.com",
                Password = "16453aA-007",
                UserName = "AddUsEmailA"
            };
            await _service.AddUser(userToAdd);
            userToAdd.UserName = "test11";

            // Act && Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.AddUser(userToAdd));
        }

        [Fact]
        public async Task AddUserWithAnAlreadyExistingUsername()
        {
            // Arrange
            var userToAdd = new AddUserDto()
            {
                Email = "AddUserAlreadyUsername@newEmail.com",
                Password = "16453aA-007",
                UserName = "AddUUnameA"
            };
            await _service.AddUser(userToAdd);
            userToAdd.Email = "RandomExistingUsername@newEmail1.com";

            // Act && Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.AddUser(userToAdd));
        }

        [Fact]
        public async Task AddUserWithNullPassword()
        {
            // Arrange
            var userToAdd = new AddUserDto()
            {
                Email = "AddUserNullPassword@newEmail3.com",
                UserName = "AddUNullPa"
            };

            // Act && Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.AddUser(userToAdd));
        }

        [Fact]
        public async Task AddUserWithNullUsername()
        {
            // Arrange
            var userToAdd = new AddUserDto()
            {
                Email = "AddUserNullUsername@newEmail3.com",
                Password = "AddUNullUs"
            };
            //_fixture.UnitOfWork.Save();

            // Act && Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.AddUser(userToAdd));
        }

        [Fact]
        public async Task AddUserWithNullEmail()
        {
            // Arrange
            var userToAdd = new AddUserDto()
            {
                UserName = "AddUNullEm",
                Password = "12345"
            };

            // Act && Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.AddUser(userToAdd));
        }

        [Fact]
        public async Task AddUserWithTooLongUsername()
        {
            // Arrange
            var userToAdd = new AddUserDto()
            {
                UserName = "aaaaaaaaaaaaaaaaaaaab",
                Email = "AddUserTooLongUsername@aNewEmail.com",
                Password = "12345"
            };

            // Act && Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.AddUser(userToAdd));
        }

        [Fact]
        public async Task AddUserWithTooShortUsername()
        {
            // Arrange
            var userToAdd = new AddUserDto()
            {
                UserName = "ab",
                Email = "AddUserTooShortUsername@aNewEmail.com",
                Password = "12345"
            };

            // Act && Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.AddUser(userToAdd));
        }

        [Fact]
        public async Task AddUserWithTooLongEmail()
        {
            // Arrange
            var userToAdd = new AddUserDto()
            {
                UserName = "AddUToLEm",
                Email = "aaaaaaaaaaaaaaaaaaaabrgffffffffffff" +
                               "rgtgthygtgtgtggtgtgtgtgtgtgtgtttttt" +
                               "ttttttttttttttttttttttttttttttttttt" +
                               "ttttttttttttttttttttttttttttttttttt" +
                               "ttttttttttttttttttttttttttttttttttt" +
                               "tttttttttttttttttttttttgggggggggggg" +
                               "gggggggggggggggggggggggggtttttttttt" +
                               "tttttggggggggggggggggtttttttttttttt" +
                               "ttggggggggggggggggttttttttttend@Email.com",
                Password = "12345"
            };

            // Act && Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.AddUser(userToAdd));
        }

        [Fact]
        public async Task AddUserWithTooLongUserDescription()
        {
            // Arrange
            var userToAdd = new AddUserDto()
            {
                UserName = "AddUToLUD",
                Email = "AddUserWithTooLongUserDescription@email12.com",
                UserDescription = "This is a long long long long long long long long long" +
                                  " long long long long long long long long long long long" +
                                  " long long long long long long long long long long long" +
                                  " long long long long long long long long long long long" +
                                  " long long long long long long long long long long long" +
                                  " long long long long long long long long long long long" +
                                  " long long long long long long long long long long long" +
                                  " long long long long long long long long long long long" +
                                  " long long long long long long long long long long long" +
                                  " long long long long long long long long long long long" +
                                  " long long long long long long long long long long long" +
                                  " long long long long long long long long long long long" +
                                  " long long long long long long long long long long long" +
                                  " long long long long long long long long long long long" +
                                  " long long long long long long long long long long long" +
                                  " long long long long long long long long long long long" +
                                  " long long long long long long long long long long long" +
                                  " long long long long long long long long long long long" +
                                  " description",
                Password = "12345"
            };

            // Act && Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.AddUser(userToAdd));
        }

        [Fact]
        public async Task GetUser()
        {
            // Arrange
            var userToAdd = new AddUserDto()
            {
                Email = "GetUser@newEmail.com",
                Password = "16453aA-007",
                UserName = "GetUser"
            };

            // Act
            var userDb = await _service.GetUser((await _service.AddUser(userToAdd)).Id);

            // Assert
            Assert.True(userDb.Email == userToAdd.Email &&
                        userDb.UserDescription == userToAdd.UserDescription &&
                        userDb.UserName == userToAdd.UserName);
        }

        [Fact]
        public async Task GetUserNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.GetUser(685479));
        }

        [Fact]
        public async Task UpdateUser()
        {
            // Arrange
            var user = await _service.AddUser(new AddUserDto()
            {
                Email = "UpdateUser@newEmail.com",
                Password = "16453aA-007",
                UserName = "UpdateUser"
            });
            var userToUpdate = new UpdateUserDto()
            {
                Id = user.Id,
                Email = "UpdateUser@newEmail.comUpdate",
                Password = "16453Update",
                UserName = "UpdateUserUpdate"
            };

            // Act
            await _service.UpdateUser(userToUpdate);
            var userUpdated = await _service.GetUser(userToUpdate.Id);

            // Assert
            Assert.True(userUpdated.Email == userToUpdate.Email &&
                        userUpdated.UserDescription == userToUpdate.UserDescription &&
                        userUpdated.UserName == userToUpdate.UserName);
        }

        [Fact]
        public async Task UpdateUserNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.UpdateUser(new UpdateUserDto() 
                {Id = 164854, Password = "123", Email = "UpdateUserNotFound@email.com", UserName = "UpdUNotFound"}));
        }

        [Fact]
        public async Task UpdateUserWithSameExistingProperties()
        {
            // Arrange
            var user = await _service.AddUser(new AddUserDto()
            {
                Email = "UpdateUserWithSameExistingProperty@newEmail.com",
                Password = "16453aA-007",
                UserName = "UpUsrWiSaExtProp"
            });
            var userToUpdate = new UpdateUserDto()
            {
                Id = user.Id,
                Email = "UpdateUserWithSameExistingProperty@newEmail.com",
                Password = "16453",
                UserName = "UpUsrWiSaExtProp"
            };

            // Act
            await _service.UpdateUser(userToUpdate);
            var userUpdated = await _service.GetUser(userToUpdate.Id);

            // Assert
            Assert.True(userUpdated.Email == userToUpdate.Email &&
                        userUpdated.UserDescription == userToUpdate.UserDescription &&
                        userUpdated.UserName == userToUpdate.UserName);
        }

        [Fact]
        public async Task UpdateUserWithSomeUniqueExistingPropertiesFromAnotherUser()
        {
            // Arrange
            await _service.AddUser(new AddUserDto()
            {
                Email = "UpdateUserWithSomeUniqueExistingPropertiesFromAnotherUser@newEmail.com",
                Password = "16453aA-007",
                UserName = "UpUsrWiSoUExtProp"
            });
            var user2 = await _service.AddUser(new AddUserDto()
            {
                Email = "UpdateUserWithSomeUExistingUProperty2@newEmail.com",
                Password = "16453aA-007",
                UserName = "UpUsrWiSoUExtProp2"
            });
            var userToUpdate = new UpdateUserDto()
            {
                Id = user2.Id,
                Email = "UpdateUserWithSomeUExistingUProperty2@newEmail.com",
                Password = "16453aA-007",
                UserName = "UpUsrWiSoUExtProp"
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.UpdateUser(userToUpdate));
        }

        [Fact]
        public async Task UpdateUserOnlyOneProperty()
        {
            // Arrange
            var user = await _service.AddUser(new AddUserDto()
            {
                Email = "UpdateUserOnlyOneProperty@newEmail.com",
                Password = "16453aA-007",
                UserName = "UpdateUOnlyOPro"
            });
            var userToUpdate = new UpdateUserDto()
            {
                Id = user.Id,
                Password = "16453",
                Email = "UpdateUserOnlyOnePropertyValid@newEmail.comUpdate",
                UserName = "UpdateUOnlyOPro"
            };

            // Act
            await _service.UpdateUser(userToUpdate);

            // Assert
            var userUpdated = await _service.GetUser(user.Id);
            Assert.True(userUpdated.Email == userToUpdate.Email &&
                        userUpdated.UserDescription == userToUpdate.UserDescription &&
                        userUpdated.UserName == userToUpdate.UserName);
        }

        [Fact]
        public async Task UpdateUserInvalid()
        {
            // Arrange
            var user = await _service.AddUser(new AddUserDto()
            {
                Email = "UpdateUserInvalid@newEmail.com",
                Password = "16453aA-007",
                UserName = "UpdateUInvalid"
            });
            var userToUpdate = new UpdateUserDto()
            {
                Id = user.Id,
                Email = "UpdateUserInvalid@newEmail.comUpdate",
                Password = "",
                UserName = "UpdateUInvalid"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.UpdateUser(userToUpdate));
        }

        [Fact]
        public async Task UpdateUserMissingUsername()
        {
            // Arrange
            var user = await _service.AddUser(new AddUserDto()
            {
                Email = "UpdateUserMissingUsername@newEmail.com",
                Password = "16453aA-007",
                UserName = "UpdateUIdMissing"
            });
            var userToUpdate = new UpdateUserDto()
            {
                Id = user.Id,
                Email = "UpdateUserMissingUsername@newEmail.comUpdate",
                Password = "16453",
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.UpdateUser(userToUpdate));
        }

        [Fact]
        public async Task DeleteUser()
        {
            // Arrange
            var user = await _service.AddUser(new AddUserDto()
            {
                Email = "DeleteUser@newEmail.com",
                Password = "16453aA-007",
                UserName = "DeleteUser"
            });

            // Act
            await _service.DeleteUser(user.Id);

            // Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.GetUser(user.Id));
        }

        [Fact]
        public async Task DeleteUserNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.DeleteUser(175574));
        }

        [Fact]
        public async Task GetAllUsers()
        {
            // Arrange
            var userToAdd = new AddUserDto()
            {
                Email = "GetAllUser@newEmail.com",
                Password = "16453aA-007",
                UserName = "GetAllUser"
            };
            var userToAdd2 = new AddUserDto()
            {
                Email = "GetAllUser2@newEmail.com",
                Password = "16453aA-007",
                UserName = "GetAllUser2"
            }; 
            await _service.AddUser(userToAdd);
            await _service.AddUser(userToAdd2);

            // Act
            var users = (await _service.GetAllUsers()).ToArray();

            // Assert
            Assert.Contains(users, x => x.UserName == userToAdd.UserName &&
                                     x.UserDescription == userToAdd.UserDescription &&
                                     x.Email == userToAdd.Email);
            Assert.Contains(users, x => x.UserName == userToAdd2.UserName &&
                                              x.UserDescription == userToAdd2.UserDescription &&
                                              x.Email == userToAdd2.Email);
        }

        [Fact]
        public async Task GetUsersFromRole()
        {
            // Arrange
            var userRole = new Role() { Name = "GetUsersFromRole" };
            await _fixture.Db.Roles.AddAsync(userRole);
            var user1 = new User()
            {
                Email = "GetUsersFromRole@email.com",
                UserName = "GetUsFromRole",
            };
            var user2 = new User()
            {
                Email = "GetUsersFromRole2@email.com",
                UserName = "GetUsFromRole2",
            };

            await _fixture.UserManager.CreateAsync(user1, "1234");
            await _fixture.UserManager.CreateAsync(user2, "1234");

            await _fixture.Db.UserRoles.AddAsync(new UserRole() { Role = userRole, User = user1 });
            await _fixture.Db.UserRoles.AddAsync(new UserRole() { Role = userRole, User = user2 });
            await _fixture.Db.SaveChangesAsync();

            // Act
            var users = (await _service.GetUsersFromRole(userRole.Id)).ToArray();

            // Assert
            Assert.True(users.Length == 2);
            Assert.Contains(users, x => x.UserName == user1.UserName &&
                                        x.UserDescription == user1.UserDescription &&
                                        x.Email == user1.Email);
            Assert.Contains(users, x => x.UserName == user2.UserName &&
                                        x.UserDescription == user2.UserDescription &&
                                        x.Email == user2.Email);
        }
    }
}
