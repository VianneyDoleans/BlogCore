using System;
using System.Linq;
using AutoMapper;
using DbAccess.Data.POCO;
using DbAccess.Data.POCO.JoiningEntity;
using DbAccess.Repositories.User;
using MyBlogAPI.DTO.User;
using MyBlogAPI.Services.UserService;
using Xunit;

namespace MyBlogAPI.Test.Services
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
            _service = new MyBlogAPI.Services.UserService.UserService(new UserRepository(_fixture.Db), 
                mapper, _fixture.UnitOfWork);
        }

        [Fact]
        public async void AddUser()
        {
            // Arrange
            var userToAdd = new AddUserDto()
            {
                EmailAddress = "AddUser@newEmail.com",
                Password = "16453",
                Username = "AddUser1"
            };

            // Act
            var user = await _service.AddUser(userToAdd);

            // Assert
            Assert.Contains(_fixture.Db.Users, x => x.Id == user.Id &&
                                                    x.EmailAddress == user.EmailAddress &&
                                                    x.UserDescription == user.UserDescription &&
                                                    x.Username == user.Username);
        }

        [Fact]
        public async void AddNullUser()
        {

            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.AddUser(null));
        }

        [Fact]
        public async void UpdateNullUser()
        {

            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.UpdateUser(null));
        }

        [Fact]
        public async void AddUserWithAnAlreadyExistingEmailAddress()
        {
            // Arrange
            var userToAdd = new AddUserDto()
            {
                EmailAddress = "AddUserAlreadyEmail@newEmail.com",
                Password = "16453",
                Username = "AddUsEmailA"
            };
            await _service.AddUser(userToAdd);
            userToAdd.Username = "test11";

            // Act && Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.AddUser(userToAdd));
        }

        [Fact]
        public async void AddUserWithAnAlreadyExistingUsername()
        {
            // Arrange
            var userToAdd = new AddUserDto()
            {
                EmailAddress = "AddUserAlreadyUsername@newEmail.com",
                Password = "16453",
                Username = "AddUUnameA"
            };
            await _service.AddUser(userToAdd);
            userToAdd.EmailAddress = "RandomExistingUsername@newEmail1.com";

            // Act && Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.AddUser(userToAdd));
        }

        [Fact]
        public async void AddUserWithNullPassword()
        {
            // Arrange
            var userToAdd = new AddUserDto()
            {
                EmailAddress = "AddUserNullPassword@newEmail3.com",
                Username = "AddUNullPa"
            };

            // Act && Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.AddUser(userToAdd));
        }

        [Fact]
        public async void AddUserWithNullUsername()
        {
            // Arrange
            var userToAdd = new AddUserDto()
            {
                EmailAddress = "AddUserNullUsername@newEmail3.com",
                Password = "AddUNullUs"
            };
            //_fixture.UnitOfWork.Save();

            // Act && Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.AddUser(userToAdd));
        }

        [Fact]
        public async void AddUserWithNullEmail()
        {
            // Arrange
            var userToAdd = new AddUserDto()
            {
                Username = "AddUNullEm",
                Password = "12345"
            };

            // Act && Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.AddUser(userToAdd));
        }

        [Fact]
        public async void AddUserWithTooLongUsername()
        {
            // Arrange
            var userToAdd = new AddUserDto()
            {
                Username = "aaaaaaaaaaaaaaaaaaaab",
                EmailAddress = "AddUserTooLongUsername@aNewEmail.com",
                Password = "12345"
            };

            // Act && Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.AddUser(userToAdd));
        }

        [Fact]
        public async void AddUserWithTooShortUsername()
        {
            // Arrange
            var userToAdd = new AddUserDto()
            {
                Username = "ab",
                EmailAddress = "AddUserTooShortUsername@aNewEmail.com",
                Password = "12345"
            };

            // Act && Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.AddUser(userToAdd));
        }

        [Fact]
        public async void AddUserWithTooLongEmail()
        {
            // Arrange
            var userToAdd = new AddUserDto()
            {
                Username = "AddUToLEm",
                EmailAddress = "aaaaaaaaaaaaaaaaaaaabrgffffffffffff" +
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
        public async void AddUserWithTooLongUserDescription()
        {
            // Arrange
            var userToAdd = new AddUserDto()
            {
                Username = "AddUToLUD",
                EmailAddress = "AddUserWithTooLongUserDescription@email12.com",
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
        public async void GetUser()
        {
            // Arrange
            var userToAdd = new AddUserDto()
            {
                EmailAddress = "GetUser@newEmail.com",
                Password = "16453",
                Username = "GetUser"
            };

            // Act
            var userDb = await _service.GetUser((await _service.AddUser(userToAdd)).Id);

            // Assert
            Assert.True(userDb.EmailAddress == userToAdd.EmailAddress &&
                        userDb.UserDescription == userToAdd.UserDescription &&
                        userDb.Username == userToAdd.Username);
        }

        [Fact]
        public async void GetUserNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.GetUser(685479));
        }

        [Fact]
        public async void UpdateUser()
        {
            // Arrange
            var user = await _service.AddUser(new AddUserDto()
            {
                EmailAddress = "UpdateUser@newEmail.com",
                Password = "16453",
                Username = "UpdateUser"
            });
            var userToUpdate = new UpdateUserDto()
            {
                Id = user.Id,
                EmailAddress = "UpdateUser@newEmail.comUpdate",
                Password = "16453Update",
                Username = "UpdateUserUpdate"
            };

            // Act
            await _service.UpdateUser(userToUpdate);
            var userUpdated = await _service.GetUser(userToUpdate.Id);

            // Assert
            Assert.True(userUpdated.EmailAddress == userToUpdate.EmailAddress &&
                        userUpdated.UserDescription == userToUpdate.UserDescription &&
                        userUpdated.Username == userToUpdate.Username);
        }

        [Fact]
        public async void UpdateUserNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.UpdateUser(new UpdateUserDto() 
                {Id = 164854, Password = "123", EmailAddress = "UpdateUserNotFound@email.com", Username = "UpdUNotFound"}));
        }

        [Fact]
        public async void UpdateUserWithSameExistingProperties()
        {
            // Arrange
            var user = await _service.AddUser(new AddUserDto()
            {
                EmailAddress = "UpdateUserWithSameExistingProperty@newEmail.com",
                Password = "16453",
                Username = "UpUsrWiSaExtProp"
            });
            var userToUpdate = new UpdateUserDto()
            {
                Id = user.Id,
                EmailAddress = "UpdateUserWithSameExistingProperty@newEmail.com",
                Password = "16453",
                Username = "UpUsrWiSaExtProp"
            };

            // Act
            await _service.UpdateUser(userToUpdate);
            var userUpdated = await _service.GetUser(userToUpdate.Id);

            // Assert
            Assert.True(userUpdated.EmailAddress == userToUpdate.EmailAddress &&
                        userUpdated.UserDescription == userToUpdate.UserDescription &&
                        userUpdated.Username == userToUpdate.Username);
        }

        [Fact]
        public async void UpdateUserWithSomeUniqueExistingPropertiesFromAnotherUser()
        {
            // Arrange
            await _service.AddUser(new AddUserDto()
            {
                EmailAddress = "UpdateUserWithSomeUniqueExistingPropertiesFromAnotherUser@newEmail.com",
                Password = "16453",
                Username = "UpUsrWiSoUExtProp"
            });
            var user2 = await _service.AddUser(new AddUserDto()
            {
                EmailAddress = "UpdateUserWithSomeUExistingUProperty2@newEmail.com",
                Password = "16453",
                Username = "UpUsrWiSoUExtProp2"
            });
            var userToUpdate = new UpdateUserDto()
            {
                Id = user2.Id,
                EmailAddress = "UpdateUserWithSomeUExistingUProperty2@newEmail.com",
                Password = "16453",
                Username = "UpUsrWiSoUExtProp"
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.UpdateUser(userToUpdate));
        }

        [Fact]
        public async void UpdateUserOnlyOneProperty()
        {
            // Arrange
            var user = await _service.AddUser(new AddUserDto()
            {
                EmailAddress = "UpdateUserOnlyOneProperty@newEmail.com",
                Password = "16453",
                Username = "UpdateUOnlyOPro"
            });
            var userToUpdate = new UpdateUserDto()
            {
                Id = user.Id,
                Password = "16453",
                EmailAddress = "UpdateUserOnlyOnePropertyValid@newEmail.comUpdate",
                Username = "UpdateUOnlyOPro"
            };

            // Act
            await _service.UpdateUser(userToUpdate);

            // Assert
            var userUpdated = await _service.GetUser(user.Id);
            Assert.True(userUpdated.EmailAddress == userToUpdate.EmailAddress &&
                        userUpdated.UserDescription == userToUpdate.UserDescription &&
                        userUpdated.Username == userToUpdate.Username);
        }

        [Fact]
        public async void UpdateUserInvalid()
        {
            // Arrange
            var user = await _service.AddUser(new AddUserDto()
            {
                EmailAddress = "UpdateUserInvalid@newEmail.com",
                Password = "16453",
                Username = "UpdateUInvalid"
            });
            var userToUpdate = new UpdateUserDto()
            {
                Id = user.Id,
                EmailAddress = "UpdateUserInvalid@newEmail.comUpdate",
                Password = "",
                Username = "UpdateUInvalid"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.UpdateUser(userToUpdate));
        }

        [Fact]
        public async void UpdateUserMissingUsername()
        {
            // Arrange
            var user = await _service.AddUser(new AddUserDto()
            {
                EmailAddress = "UpdateUserMissingUsername@newEmail.com",
                Password = "16453",
                Username = "UpdateUIdMissing"
            });
            var userToUpdate = new UpdateUserDto()
            {
                Id = user.Id,
                EmailAddress = "UpdateUserMissingUsername@newEmail.comUpdate",
                Password = "16453",
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await _service.UpdateUser(userToUpdate));
        }

        [Fact]
        public async void DeleteUser()
        {
            // Arrange
            var user = await _service.AddUser(new AddUserDto()
            {
                EmailAddress = "UpdateUser@newEmail.com",
                Password = "16453",
                Username = "UpdateUser"
            });

            // Act
            await _service.DeleteUser(user.Id);

            // Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.GetUser(user.Id));
        }

        [Fact]
        public async void DeleteUserNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.DeleteUser(175574));
        }

        [Fact]
        public async void GetAllUsers()
        {
            // Arrange
            var userToAdd = new AddUserDto()
            {
                EmailAddress = "GetAllUser@newEmail.com",
                Password = "16453",
                Username = "GetAllUser"
            };
            var userToAdd2 = new AddUserDto()
            {
                EmailAddress = "GetAllUser2@newEmail.com",
                Password = "16453",
                Username = "GetAllUser2"
            }; 
            await _service.AddUser(userToAdd);
            await _service.AddUser(userToAdd2);

            // Act
            var users = (await _service.GetAllUsers()).ToArray();

            // Assert
            Assert.Contains(users, x => x.Username == userToAdd.Username &&
                                     x.UserDescription == userToAdd.UserDescription &&
                                     x.EmailAddress == userToAdd.EmailAddress);
            Assert.Contains(users, x => x.Username == userToAdd2.Username &&
                                              x.UserDescription == userToAdd2.UserDescription &&
                                              x.EmailAddress == userToAdd2.EmailAddress);
        }

        [Fact]
        public async void GetUsersFromRole()
        {
            // Arrange
            var userRole = new Role() { Name = "GetUsersFromRole" };
            await _fixture.Db.Roles.AddAsync(userRole);
            var user1 = new User()
                {
                    EmailAddress = "GetUsersFromRole@email.com",
                    Username = "GetUsFromRole",
                    Password = "1234"
                };
            var user2 = new User()
            {
                EmailAddress = "GetUsersFromRole2@email.com",
                Username = "GetUsFromRole2",
                Password = "1234"
            };
            await _fixture.Db.Users.AddAsync(user1);
            await _fixture.Db.Users.AddAsync(user2);

            await _fixture.Db.UserRoles.AddAsync(new UserRole() { Role = userRole, User = user1 });
            await _fixture.Db.UserRoles.AddAsync(new UserRole() { Role = userRole, User = user2 });
            await _fixture.Db.SaveChangesAsync();

            // Act
            var users = (await _service.GetUsersFromRole(userRole.Id)).ToArray();

            // Assert
            Assert.True(users.Length == 2);
            Assert.Contains(users, x => x.Username == user1.Username &&
                                        x.UserDescription == user1.UserDescription &&
                                        x.EmailAddress == user1.EmailAddress);
            Assert.Contains(users, x => x.Username == user2.Username &&
                                        x.UserDescription == user2.UserDescription &&
                                        x.EmailAddress == user2.EmailAddress);
        }
    }
}
