using System;
using System.Linq;
using System.Net.Http;
using MyBlogAPI.DTO.User;

namespace MyBlogAPI.IntegrationTests.Helpers
{
    public class UserHelper : AEntityHelper<GetUserDto, AddUserDto, UpdateUserDto>
    {
        public UserHelper(HttpClient client, string baseUrl = "/users") : base(baseUrl, client)
        {
        }

        protected override AddUserDto CreateTAdd()
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

        protected override UpdateUserDto ModifyTUpdate(UpdateUserDto entity)
        {
            return new UpdateUserDto()
            {
                EmailAddress = entity.EmailAddress,
                Id = entity.Id,
                Password = entity.Password,
                UserDescription = Guid.NewGuid().ToString(),
                Username = entity.Username
            };
        }

        public override bool Equals(GetUserDto first, GetUserDto second)
        {
            if (first == null || second == null)
                return false;
            if (first.Roles == null || second.Roles == null)
                return first.EmailAddress == second.EmailAddress &&
                       first.LastLogin == second.LastLogin &&
                       first.RegisteredAt == second.RegisteredAt &&
                       first.UserDescription == second.UserDescription &&
                       first.Username == second.Username;
            return first.Roles.SequenceEqual(second.Roles);
        }
    }
}
