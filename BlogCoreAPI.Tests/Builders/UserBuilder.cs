using System;
using System.Threading.Tasks;
using BlogCoreAPI.Models.DTOs.User;
using BlogCoreAPI.Services.UserService;

namespace BlogCoreAPI.Tests.Builders
{
    public class UserBuilder
    {
        private readonly IUserService _userService;

        public UserBuilder(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<GetUserDto> Build()
        {
            var userToAdd = new AddUserDto()
            {
                Email = Guid.NewGuid().ToString("N") + "@test.com",
                Password = "16453aA-007",
                UserName = Guid.NewGuid().ToString()[..20]
            };
            var user = await _userService.AddUser(userToAdd);
            return user;
        }
    }
}
