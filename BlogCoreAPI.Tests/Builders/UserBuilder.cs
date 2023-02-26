using System;
using System.Threading.Tasks;
using BlogCoreAPI.Models.DTOs.Account;
using BlogCoreAPI.Models.DTOs.User;
using BlogCoreAPI.Services.UserService;

namespace BlogCoreAPI.Tests.Builders
{
    public class AccountBuilder
    {
        private readonly IUserService _userService;

        public AccountBuilder(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<GetAccountDto> Build()
        {
            var userToAdd = new AddAccountDto()
            {
                Email = Guid.NewGuid().ToString("N") + "@test.com",
                Password = "16453aA-007",
                UserName = Guid.NewGuid().ToString()[..20]
            };
            var user = await _userService.AddAccount(userToAdd);
            return user;
        }
    }
}
