using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MyBlogAPI.DTO.User;
using MyBlogAPI.IntegrationTests.Helpers;
using Newtonsoft.Json;
using Xunit;

namespace MyBlogAPI.IntegrationTests
{
    public class UsersController : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly UserHelper _helper;

        public UsersController(TestWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _helper = new UserHelper(_client);
        }

        [Fact]
        public async Task CanGetUsers()
        {
            var users = await _helper.GetAll();

            Assert.True(users.Any());
        }

        [Fact]
        public async Task CanAddUser()
        {
            var user = await _helper.AddRandomEntity();

            // Get Users
            var httpGetResponse = await _client.GetAsync("/users");
            httpGetResponse.EnsureSuccessStatusCode();

            // Deserialize and compare
            var stringResponse = await httpGetResponse.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<IEnumerable<GetUserDto>>(stringResponse);
            Assert.Contains(users, p => p.Username == user.Username
                                        && p.UserDescription == user.UserDescription
                                        && p.EmailAddress == user.EmailAddress);
        }

        [Fact]
        public async Task CanGetUser()
        {
            var user = await _helper.AddRandomEntity();
            var users = await _helper.GetAll();
            var userAdded = users.Single(p => p.Username == user.Username
                                              && p.UserDescription == user.UserDescription
                                              && p.EmailAddress == user.EmailAddress);
            var result = await _helper.GetById(userAdded.Id);

            Assert.True(result.Username == userAdded.Username 
                               && result.EmailAddress == userAdded.EmailAddress
                               && result.UserDescription == userAdded.UserDescription);
        }

        [Fact]
        public async Task CanUpdateUser()
        {
            var userToAdd = new AddUserDto()
            {
                EmailAddress = "AddANewUser@user.com",
                Password = "abcdh",
                UserDescription = "My description",
                Username = "UserB"
            };
            await _helper.AddEntity(userToAdd);

            // Recover the user added inside the DB to obtain its Id assigned
            var users = await _helper.GetAll();
            var userAdded = users.Single(p => p.Username == userToAdd.Username
                                              && p.UserDescription == userToAdd.UserDescription
                                              && p.EmailAddress == userToAdd.EmailAddress);
            var userToUpdate = new AddUserDto()
            {
                EmailAddress = userAdded.EmailAddress,
                Id = userAdded.Id,
                Password = Guid.NewGuid().ToString(),
                UserDescription = userAdded.UserDescription,
                Username = Guid.NewGuid().ToString()
            };
            await _helper.UpdateIdentity(userToUpdate);

            // Compare
            users = await _helper.GetAll();
            Assert.Contains(users, p => p.Id == userToUpdate.Id
                                        && p.Username == userToUpdate.Username
                                        && p.UserDescription == userToUpdate.UserDescription
                                        && p.EmailAddress == userToUpdate.EmailAddress);
        }

        [Fact]
        public async Task CanDeleteUser()
        {
            var user = await _helper.AddRandomEntity();
            var users = await _helper.GetAll();
            var userAdded = users.Single(p => p.Username == user.Username
                                              && p.UserDescription == user.UserDescription
                                              && p.EmailAddress == user.EmailAddress);
            var result = await _helper.GetById(userAdded.Id);

            await _helper.RemoveIdentity(result.Id);
            Assert.DoesNotContain(await _helper.GetAll(), p => p.Id == result.Id);
        }
    }
}
