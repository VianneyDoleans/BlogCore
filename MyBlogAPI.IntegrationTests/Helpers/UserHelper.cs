using System;
using System.Net.Http;
using MyBlogAPI.DTO.User;

namespace MyBlogAPI.IntegrationTests.Helpers
{
    public class UserHelper : GenericEntityHelper<GetUserDto, AddUserDto>
    {
        public UserHelper(HttpClient client, string baseUrl = "/users") : base(baseUrl, client)
        {
        }

        protected override AddUserDto CreateAddEntity()
        {
            var user = new AddUserDto()
            {
                EmailAddress = "AddANewUser@user.com",
                Password = "abcdh",
                UserDescription = "My description",
                Username = Guid.NewGuid().ToString()
            };
            return user;
        }
    }
}
