using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MyBlogAPI.DTOs.User;

namespace MyBlogAPI.FunctionalTests.Helpers
{
    public class UserHelper : AEntityHelper<GetUserDto, AddUserDto, UpdateUserDto>
    {
        private readonly AccountHelper _accountHelper;

        public UserHelper(HttpClient client, string baseUrl = "/users") : base(baseUrl, client)
        {
            _accountHelper = new AccountHelper(client);
        }

        public override async Task<GetUserDto> AddEntity(AddUserDto entity)
        {
            var userCreated = await _accountHelper.CreateAccount(entity);
            return userCreated;
        }

        protected override UpdateUserDto ModifyTUpdate(UpdateUserDto entity)
        {
            return new UpdateUserDto()
            {
                Email = entity.Email,
                Id = entity.Id,
                Password = Guid.NewGuid().ToString("N"),
                UserDescription = entity.UserDescription,
                UserName = Guid.NewGuid().ToString("N")[..20]
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
                return first.Email == second.Email &&
                       first.LastLogin == second.LastLogin &&
                       first.RegisteredAt == second.RegisteredAt &&
                       first.UserDescription == second.UserDescription &&
                       first.UserName == second.UserName;
            return first.Roles.SequenceEqual(second.Roles) &&
                   first.Email == second.Email &&
                   first.LastLogin == second.LastLogin &&
                   first.RegisteredAt == second.RegisteredAt &&
                   first.UserDescription == second.UserDescription &&
                   first.UserName == second.UserName;
        }
    }
}
