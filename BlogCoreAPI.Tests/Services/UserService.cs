using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlogCoreAPI.Models.DTOs.Account;
using BlogCoreAPI.Models.DTOs.User;
using BlogCoreAPI.Models.Exceptions;
using BlogCoreAPI.Services.UserService;
using BlogCoreAPI.Validators.User;
using DBAccess.Data;
using DBAccess.Data.JoiningEntity;
using DBAccess.Exceptions;
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
        public async Task AddAccount()
        {
            // Arrange
            var accountToAdd = new AddAccountDto()
            {
                Email = "AddAccount@newEmail.com",
                Password = "16453aA-007",
                UserName = "AddAccount1"
            };

            // Act
            var account = await _service.AddAccount(accountToAdd);

            // Assert
            Assert.Contains(_fixture.Db.Users, x => x.Id == account.Id &&
                                                    x.Email == account.Email &&
                                                    x.UserDescription == account.UserDescription &&
                                                    x.UserName == account.UserName);
        }

        [Fact]
        public async Task AddNullAccount()
        {

            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.AddAccount(null));
        }

        [Fact]
        public async Task UpdateNullAccount()
        {

            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.UpdateAccount(null));
        }

        [Fact]
        public async Task AddAccountWithAnAlreadyExistingEmailAddress()
        {
            // Arrange
            var accountToAdd = new AddAccountDto()
            {
                Email = "AddAccountAlreadyEmail@newEmail.com",
                Password = "16453aA-007",
                UserName = "AddUsEmailA"
            };
            await _service.AddAccount(accountToAdd);
            accountToAdd.UserName = "test11";

            // Act && Assert
            await Assert.ThrowsAsync<InvalidRequestException>(async () => await _service.AddAccount(accountToAdd));
        }

        [Fact]
        public async Task AddAccountWithAnAlreadyExistingUsername()
        {
            // Arrange
            var accountToAdd = new AddAccountDto()
            {
                Email = "AddAccountAlreadyUsername@newEmail.com",
                Password = "16453aA-007",
                UserName = "AddUUnameA"
            };
            await _service.AddAccount(accountToAdd);
            accountToAdd.Email = "RandomExistingUsername@newEmail1.com";

            // Act && Assert
            await Assert.ThrowsAsync<InvalidRequestException>(async () => await _service.AddAccount(accountToAdd));
        }

        [Fact]
        public async Task AddAccountWithNullPassword()
        {
            // Arrange
            var accountToAdd = new AddAccountDto()
            {
                Email = "AddAccountNullPassword@newEmail3.com",
                UserName = "AddUNullPa"
            };

            // Act && Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.AddAccount(accountToAdd));
        }

        [Fact]
        public async Task AddAccountWithNullUsername()
        {
            // Arrange
            var accountToAdd = new AddAccountDto()
            {
                Email = "AddAccountNullUsername@newEmail3.com",
                Password = "AddUNullUs"
            };

            // Act && Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.AddAccount(accountToAdd));
        }

        [Fact]
        public async Task AddAccountWithNullEmail()
        {
            // Arrange
            var accountToAdd = new AddAccountDto()
            {
                UserName = "AddUNullEm",
                Password = "12345"
            };

            // Act && Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.AddAccount(accountToAdd));
        }

        [Fact]
        public async Task AddAccountWithTooLongUsername()
        {
            // Arrange
            var accountToAdd = new AddAccountDto()
            {
                UserName = "aaaaaaaaaaaaaaaaaaaab",
                Email = "AddAccountTooLongUsername@aNewEmail.com",
                Password = "12345"
            };

            // Act && Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.AddAccount(accountToAdd));
        }

        [Fact]
        public async Task AddAccountWithTooShortUsername()
        {
            // Arrange
            var accountToAdd = new AddAccountDto()
            {
                UserName = "ab",
                Email = "AddAccountTooShortUsername@aNewEmail.com",
                Password = "12345"
            };

            // Act && Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.AddAccount(accountToAdd));
        }

        [Fact]
        public async Task AddAccountWithTooLongEmail()
        {
            // Arrange
            var accountToAdd = new AddAccountDto()
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
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.AddAccount(accountToAdd));
        }

        [Fact]
        public async Task AddAccountWithTooLongUserDescription()
        {
            // Arrange
            var accountToAdd = new AddAccountDto()
            {
                UserName = "AddUToLUD",
                Email = "AddAccountWithTooLongUserDescription@email12.com",
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
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.AddAccount(accountToAdd));
        }

        [Fact]
        public async Task GetAccount()
        {
            // Arrange
            var accountToAdd = new AddAccountDto()
            {
                Email = "GetUser@newEmail.com",
                Password = "16453aA-007",
                UserName = "GetUser"
            };

            // Act
            var accountDb = await _service.GetAccount((await _service.AddAccount(accountToAdd)).Id);

            // Assert
            Assert.True(accountDb.Email == accountToAdd.Email &&
                        accountDb.UserDescription == accountToAdd.UserDescription &&
                        accountDb.UserName == accountToAdd.UserName);
        }

        [Fact]
        public async Task GetAccountNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _service.GetAccount(685479));
        }

        [Fact]
        public async Task UpdateAccount()
        {
            // Arrange
            var account = await _service.AddAccount(new AddAccountDto()
            {
                Email = "UpdateAccount@newEmail.com",
                Password = "16453aA-007",
                UserName = "UpdateAccount"
            });
            var userToUpdate = new UpdateAccountDto()
            {
                Id = account.Id,
                Email = "UpdateAccount@newEmail.comUpdate",
                Password = "16453Update",
                UserName = "UpdateAccountUpdate"
            };

            // Act
            await _service.UpdateAccount(userToUpdate);
            var userUpdated = await _service.GetAccount(userToUpdate.Id);

            // Assert
            Assert.True(userUpdated.Email == userToUpdate.Email &&
                        userUpdated.UserDescription == userToUpdate.UserDescription &&
                        userUpdated.UserName == userToUpdate.UserName);
        }

        [Fact]
        public async Task UpdateAccountNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _service.UpdateAccount(new UpdateAccountDto() 
                {Id = 164854, Password = "123", Email = "UpdateAccountNotFound@email.com", UserName = "UpdUNotFound"}));
        }

        [Fact]
        public async Task UpdateAccountWithSameExistingProperties()
        {
            // Arrange
            var user = await _service.AddAccount(new AddAccountDto()
            {
                Email = "UpdateAccountWithSameExistingProperty@newEmail.com",
                Password = "16453aA-007",
                UserName = "UpUsrWiSaExtProp"
            });
            var userToUpdate = new UpdateAccountDto()
            {
                Id = user.Id,
                Email = "UpdateAccountWithSameExistingProperty@newEmail.com",
                Password = "16453",
                UserName = "UpUsrWiSaExtProp"
            };

            // Act
            await _service.UpdateAccount(userToUpdate);
            var accountUpdated = await _service.GetAccount(userToUpdate.Id);

            // Assert
            Assert.True(accountUpdated.Email == userToUpdate.Email &&
                        accountUpdated.UserDescription == userToUpdate.UserDescription &&
                        accountUpdated.UserName == userToUpdate.UserName);
        }

        [Fact]
        public async Task UpdateAccountWithSomeUniqueExistingPropertiesFromAnotherUser()
        {
            // Arrange
            await _service.AddAccount(new AddAccountDto()
            {
                Email = "UpdateAccountWithSomeUniqueExistingPropertiesFromAnotherUser@newEmail.com",
                Password = "16453aA-007",
                UserName = "UpUsrWiSoUExtProp"
            });
            var account2 = await _service.AddAccount(new AddAccountDto()
            {
                Email = "UpdateAccountWithSomeUExistingUProperty2@newEmail.com",
                Password = "16453aA-007",
                UserName = "UpUsrWiSoUExtProp2"
            });
            var userToUpdate = new UpdateAccountDto()
            {
                Id = account2.Id,
                Email = "UpdateAccountWithSomeUExistingUProperty2@newEmail.com",
                Password = "16453aA-007",
                UserName = "UpUsrWiSoUExtProp"
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidRequestException>(async () => await _service.UpdateAccount(userToUpdate));
        }

        [Fact]
        public async Task UpdateAccountOnlyOneProperty()
        {
            // Arrange
            var account = await _service.AddAccount(new AddAccountDto()
            {
                Email = "UpdateAccountOnlyOneProperty@newEmail.com",
                Password = "16453aA-007",
                UserName = "UpdateUOnlyOPro"
            });
            var userToUpdate = new UpdateAccountDto()
            {
                Id = account.Id,
                Password = "16453",
                Email = "UpdateAccountOnlyOnePropertyValid@newEmail.comUpdate",
                UserName = "UpdateUOnlyOPro"
            };

            // Act
            await _service.UpdateAccount(userToUpdate);

            // Assert
            var accountUpdated = await _service.GetAccount(account.Id);
            Assert.True(accountUpdated.Email == userToUpdate.Email &&
                        accountUpdated.UserDescription == userToUpdate.UserDescription &&
                        accountUpdated.UserName == userToUpdate.UserName);
        }

        [Fact]
        public async Task UpdateAccountInvalid()
        {
            // Arrange
            var account = await _service.AddAccount(new AddAccountDto()
            {
                Email = "UpdateAccountInvalid@newEmail.com",
                Password = "16453aA-007",
                UserName = "UpdateUInvalid"
            });
            var userToUpdate = new UpdateAccountDto()
            {
                Id = account.Id,
                Email = "UpdateAccountInvalid@newEmail.comUpdate",
                Password = "",
                UserName = "UpdateUInvalid"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.UpdateAccount(userToUpdate));
        }

        [Fact]
        public async Task UpdateAccountMissingUsername()
        {
            // Arrange
            var account = await _service.AddAccount(new AddAccountDto()
            {
                Email = "UpdateAccountMissingUsername@newEmail.com",
                Password = "16453aA-007",
                UserName = "UpdateUIdMissing"
            });
            var userToUpdate = new UpdateAccountDto()
            {
                Id = account.Id,
                Email = "UpdateAccountMissingUsername@newEmail.comUpdate",
                Password = "16453",
            };

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(async () => await _service.UpdateAccount(userToUpdate));
        }

        [Fact]
        public async Task DeleteUser()
        {
            // Arrange
            var account = await _service.AddAccount(new AddAccountDto()
            {
                Email = "DeleteUser@newEmail.com",
                Password = "16453aA-007",
                UserName = "DeleteUser"
            });

            // Act
            await _service.DeleteAccount(account.Id);

            // Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _service.GetAccount(account.Id));
        }

        [Fact]
        public async Task DeleteUserNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _service.DeleteAccount(175574));
        }

        [Fact]
        public async Task GetAllAccounts()
        {
            // Arrange
            var accountToAdd = new AddAccountDto()
            {
                Email = "GetAllUser@newEmail.com",
                Password = "16453aA-007",
                UserName = "GetAllUser"
            };
            var accountToAdd2 = new AddAccountDto()
            {
                Email = "GetAllUser2@newEmail.com",
                Password = "16453aA-007",
                UserName = "GetAllUser2"
            }; 
            await _service.AddAccount(accountToAdd);
            await _service.AddAccount(accountToAdd2);

            // Act
            var accounts = (await _service.GetAllAccounts()).ToArray();

            // Assert
            Assert.Contains(accounts, x => x.UserName == accountToAdd.UserName &&
                                     x.UserDescription == accountToAdd.UserDescription &&
                                     x.Email == accountToAdd.Email);
            Assert.Contains(accounts, x => x.UserName == accountToAdd2.UserName &&
                                              x.UserDescription == accountToAdd2.UserDescription &&
                                              x.Email == accountToAdd2.Email);
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
                                        x.UserDescription == user1.UserDescription);
            Assert.Contains(users, x => x.UserName == user2.UserName &&
                                        x.UserDescription == user2.UserDescription);
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
            await Assert.ThrowsAsync<InvalidRequestException>(async () => await _service.AddUserRole(new UserRoleDto()
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
            await Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _service.AddUserRole(new UserRoleDto()
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
            await Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _service.AddUserRole(new UserRoleDto()
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
            await Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _service.RemoveUserRole(new UserRoleDto()
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
            await Assert.ThrowsAsync<InvalidRequestException>(async () => await _service.RemoveUserRole(new UserRoleDto()
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
            await Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _service.RemoveUserRole(new UserRoleDto()
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
            var isSignIn = await _service.SignIn(new AccountLoginDto() { UserName = user.UserName, Password = password });

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
            var isSignIn = await _service.SignIn(new AccountLoginDto() { UserName = user.UserName, Password = "wrongPass1234Ab@a" });

            // Assert
            Assert.False(isSignIn);
        }

        [Fact]
        public async Task SignInUserNameDoesNotExist()
        {
            // Arrange
            const string password = "1234Ab@a";

            // Act & Assert
            await Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _service.SignIn(new AccountLoginDto()
                { UserName = Guid.NewGuid().ToString()[..20], Password = password }));
        }
    }
}
