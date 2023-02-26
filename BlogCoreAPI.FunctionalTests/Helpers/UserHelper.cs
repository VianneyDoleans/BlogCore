using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BlogCoreAPI.Models.DTOs.Account;
using Newtonsoft.Json;

namespace BlogCoreAPI.FunctionalTests.Helpers
{
    public class UserHelper : AEntityHelper<GetAccountDto, AddAccountDto, UpdateAccountDto>
    {
        private readonly AccountHelper _accountHelper;

        public UserHelper(HttpClient client, string baseUrl = "/users") : base(baseUrl, client)
        {
            _accountHelper = new AccountHelper(client);
        }

        public override async Task<GetAccountDto> AddEntity(AddAccountDto entity)
        {
            var userCreated = await _accountHelper.CreateAccount(entity);
            return userCreated;
        }

        public override async Task RemoveIdentity(int id)
        {
            await _accountHelper.DeleteAccount(id);
        }

        public override async Task UpdateEntity(UpdateAccountDto entity)
        {
            await _accountHelper.UpdateAccount(entity);
        }

        protected override UpdateAccountDto ModifyTUpdate(UpdateAccountDto entity)
        {
            return new UpdateAccountDto()
            {
                Email = entity.Email,
                Id = entity.Id,
                Password = Guid.NewGuid().ToString("N"),
                UserDescription = entity.UserDescription,
                UserName = Guid.NewGuid().ToString("N")[..20]
            };
        }

        public override bool Equals(GetAccountDto first, GetAccountDto second)
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
