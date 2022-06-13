using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlogCoreAPI.DTOs.User;
using BlogCoreAPI.Services.UserService;
using BlogCoreAPI.Validators.User;
using DBAccess.Data;
using DBAccess.Data.JoiningEntity;
using DBAccess.Repositories.Role;
using DBAccess.Repositories.User;
using FluentValidation;
using Xunit;

namespace BlogCoreAPI.Tests.Services
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
            _service = new BlogCoreAPI.Services.UserService.UserService(new UserRepository(_fixture.Db, _fixture.UserManager), new RoleRepository(_fixture.Db, _fixture.RoleManager), 
                mapper, _fixture.UnitOfWork, new UserDtoValidator());
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
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.AddUser(userToAdd));
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

            // Act && Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.AddUser(userToAdd));
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
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.AddUser(userToAdd));
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
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.AddUser(userToAdd));
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
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.AddUser(userToAdd));
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
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.AddUser(userToAdd));
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
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.AddUser(userToAdd));
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
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.UpdateUser(userToUpdate));
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
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.UpdateUser(userToUpdate));
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

        [Fact]
        public async Task AddRoleToUser()
        {
            // Arrange
            var user = new User()
            {
                Email = "AddRoleToUser@email.com",
                UserName = "AddRoleToUser",
            };
            var role = new Role() { Name = "AddRoleToUser" };

            await _fixture.UserManager.CreateAsync(user, "1234Ab@a");
            await _fixture.RoleManager.CreateAsync(role);
            var userId = (await _fixture.UserManager.FindByNameAsync(user.UserName)).Id;
            var roleId = (await _fixture.RoleManager.FindByNameAsync(role.Name)).Id;

            // Act
            var exception = await Record.ExceptionAsync(async () => await _service.AddUserRole(new UserRoleDto() {
                RoleId = roleId, 
                UserId = userId
            }));

            // Assert
            Assert.Null(exception);
            Assert.True((await _fixture.UserManager.FindByNameAsync(user.UserName)).UserRoles.Any());
            Assert.Contains((await _fixture.UserManager.FindByNameAsync(user.UserName)).UserRoles, x => x.RoleId == roleId && x.UserId == userId);
        }

        [Fact]
        public async Task AddRoleToUserWithUserAlreadyHaveTheRole()
        {
            // Arrange
            var user = new User()
            {
                Email = "AddRToUWiUAlHaThR@email.com",
                UserName = "AddRToUWiUAlHaTh",
            };
            var role = new Role() { Name = "AddRToUWiUAlHaTh" };

            await _fixture.UserManager.CreateAsync(user, "1234Ab@a");
            await _fixture.RoleManager.CreateAsync(role);
            var userId = (await _fixture.UserManager.FindByNameAsync(user.UserName)).Id;
            var roleId = (await _fixture.RoleManager.FindByNameAsync(role.Name)).Id;
            await _service.AddUserRole(new UserRoleDto()
            {
                RoleId = roleId,
                UserId = userId
            });

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.AddUserRole(new UserRoleDto()
            {
                RoleId = roleId,
                UserId = userId
            }));
        }

        [Fact]
        public async Task AddRoleToUserWithRoleDoesNotExist()
        {
            // Arrange
            var user = new User()
            {
                Email = "AddRToUsWiRoDoNoEx",
                UserName = "AddRToUsWiRoDoNoEx@email.com",
            };

            await _fixture.UserManager.CreateAsync(user, "1234Ab@a");
            var userId = (await _fixture.UserManager.FindByNameAsync(user.UserName)).Id;

            // Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.AddUserRole(new UserRoleDto()
            {
                RoleId = 78542,
                UserId = userId
            }));
        }

        [Fact]
        public async Task AddRoleToUserWithUserDoesNotExist()
        {
            // Arrange
            var role = new Role() { Name = "AddRTUWithUDoNoEx" };

            await _fixture.RoleManager.CreateAsync(role);
            var roleId = (await _fixture.RoleManager.FindByNameAsync(role.Name)).Id;

            // Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.AddUserRole(new UserRoleDto()
            {
                RoleId = roleId,
                UserId = 323365
            }));
        }

        [Fact]
        public async Task RemoveRoleToUser()
        {
            // Arrange
            var user = new User()
            {
                Email = "ReRoleToUser@email.com",
                UserName = "ReRoleToUser",
            };
            var role = new Role() { Name = "ReRoleToUser" };

            await _fixture.UserManager.CreateAsync(user, "1234Ab@a");
            await _fixture.RoleManager.CreateAsync(role);
            var userId = (await _fixture.UserManager.FindByNameAsync(user.UserName)).Id;
            var roleId = (await _fixture.RoleManager.FindByNameAsync(role.Name)).Id; 
            await _service.AddUserRole(new UserRoleDto()
            {
                RoleId = roleId,
                UserId = userId
            });

            // Act
            var exception = await Record.ExceptionAsync(async () => await _service.RemoveUserRole(new UserRoleDto()
            {
                RoleId = roleId,
                UserId = userId
            }));

            // Assert
            Assert.Null(exception);
            Assert.DoesNotContain((await _fixture.UserManager.FindByNameAsync(user.UserName)).UserRoles, x => x.RoleId == roleId && x.UserId == userId);
        }

        [Fact]
        public async Task RemoveRoleToUserWithRoleDoesNotExist()
        {
            // Arrange
            var user = new User()
            {
                Email = "ReRToUWiUAlHaThR@email.com",
                UserName = "ReRToUWiUAlHaTh",
            };

            await _fixture.UserManager.CreateAsync(user, "1234Ab@a");
            var userId = (await _fixture.UserManager.FindByNameAsync(user.UserName)).Id;

            // Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.RemoveUserRole(new UserRoleDto()
            {
                RoleId = 1234432,
                UserId = userId
            }));
        }

        [Fact]
        public async Task RemoveRoleToUserWithUserDoesNotHaveTheRole()
        {
            // Arrange
            var user = new User()
            {
                Email = "ReRToUsWiRoDoNoEx@email.com",
                UserName = "ReRToUWiRoDoNEx",
            };
            var role = new Role() { Name = "ReRoleToUser" };

            await _fixture.RoleManager.CreateAsync(role);
            var roleId = (await _fixture.RoleManager.FindByNameAsync(role.Name)).Id;

            await _fixture.UserManager.CreateAsync(user, "1234Ab@a");
            var userId = (await _fixture.UserManager.FindByNameAsync(user.UserName)).Id;

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.RemoveUserRole(new UserRoleDto()
            {
                RoleId = roleId,
                UserId = userId
            }));
        }

        [Fact]
        public async Task RemoveRoleToUserWithUserDoesNotExist()
        {
            // Arrange
            var role = new Role() { Name = "ReRTUWithUDoNoEx" };

            await _fixture.RoleManager.CreateAsync(role);
            var roleId = (await _fixture.RoleManager.FindByNameAsync(role.Name)).Id;

            // Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.RemoveUserRole(new UserRoleDto()
            {
                RoleId = roleId,
                UserId = 323365
            }));
        }

        [Fact]
        public async Task SignIn()
        {
            // Arrange
            var user = new User()
            {
                Email = "SignInTes",
                UserName = "SignInTes@email.com",
            };
            const string password = "1234Ab@a";
            await _fixture.UserManager.CreateAsync(user, password);

            // Act
            var isSignIn = await _service.SignIn(new UserLoginDto() { UserName = user.UserName, Password = password });

            // Assert
            Assert.True(isSignIn);
        }

        [Fact]
        public async Task SignInNotCorrect()
        {
            // Arrange
            var user = new User()
            {
                Email = "SignInTesNoCo",
                UserName = "SignInTesNoCorrect@email.com",
            };
            const string password = "1234Ab@a";
            await _fixture.UserManager.CreateAsync(user, password);

            // Act
            var isSignIn = await _service.SignIn(new UserLoginDto() { UserName = user.UserName, Password = "wrongPass1234Ab@a" });

            // Assert
            Assert.False(isSignIn);
        }

        [Fact]
        public async Task SignInUserNameDoesNotExist()
        {
            // Arrange
            const string password = "1234Ab@a";

            // Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.SignIn(new UserLoginDto()
                { UserName = Guid.NewGuid().ToString()[..20], Password = password }));
        }
    }
}
