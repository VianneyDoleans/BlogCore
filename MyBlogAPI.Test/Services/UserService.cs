using System;
using AutoMapper;
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
                EmailAddress = "test@newEmail.com",
                Password = "16453",
                Username = "test10"
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
        public async void AddUserWithAnAlreadyExistingEmailAddress()
        {
            // Arrange
            var userToAdd = new AddUserDto()
            {
                EmailAddress = "test@newEmail.com",
                Password = "16453",
                Username = "test10"
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
                EmailAddress = "test@newEmail.com",
                Password = "16453",
                Username = "test10"
            };
            await _service.AddUser(userToAdd);
            userToAdd.EmailAddress = "test@newEmail1.com";

            // Act && Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _service.AddUser(userToAdd));
        }

        [Fact]
        public async void AddUserWithNullPassword()
        {
            // Arrange
            var userToAdd = new AddUserDto()
            {
                EmailAddress = "test@newEmail3.com",
                Username = "test10"
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
                EmailAddress = "test@newEmail3.com",
                Password = "12345"
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
                Username = "okTest",
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
                EmailAddress = "test@aNewEmail.com",
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
                EmailAddress = "test@aNewEmail.com",
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
                Username = "test2TestAdd",
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
                Username = "test12AddNew",
                EmailAddress = "123435@email12.com",
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
        public async void GetUserNotFound()
        {
            // Arrange & Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(async () => await _service.GetUser(685479));
        }
    }
}
