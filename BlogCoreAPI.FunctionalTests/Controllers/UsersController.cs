using System;
using System.Threading.Tasks;
using MyBlogAPI.DTOs.User;
using MyBlogAPI.FunctionalTests.GenericTests;
using MyBlogAPI.FunctionalTests.Helpers;
using Xunit;

namespace MyBlogAPI.FunctionalTests.Controllers
{
    public sealed class UsersController : AGenericTests<GetUserDto, AddUserDto, UpdateUserDto>, IClassFixture<TestWebApplicationFactory>
    {
        protected override IEntityHelper<GetUserDto, AddUserDto, UpdateUserDto> Helper { get; set; }
        public override async Task<GetUserDto> AddRandomEntity()
        {
            var user = new AddUserDto()
            {
                Email = Guid.NewGuid().ToString("N") + "@user.com",
                Password = "0a1234A@",
                UserDescription = "My description",
                UserName = Guid.NewGuid().ToString()[..20]
            };
            return await Helper.AddEntity(user);
        }

        public UsersController(TestWebApplicationFactory factory) : base(factory)
        {
            Helper = new UserHelper(Client);
        }
    }
}
