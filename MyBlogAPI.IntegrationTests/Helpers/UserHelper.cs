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

        protected override UpdateUserDto ModifyTUpdate(UpdateUserDto entity)
        {
            return new UpdateUserDto()
            {
                EmailAddress = entity.EmailAddress,
                Id = entity.Id,
                Password = Guid.NewGuid().ToString("N"),
                UserDescription = entity.UserDescription,
                Username = Guid.NewGuid().ToString("N")[..20]
            };
        }

        public override bool Equals(GetUserDto first, GetUserDto second)
        {
            if (first == null || second == null)
                return false;
            if (first.Roles == null && second.Roles != null ||
                first.Roles != null && second.Roles == null)
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
