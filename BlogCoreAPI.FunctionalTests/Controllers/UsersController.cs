using System;
using System.Threading.Tasks;
using BlogCoreAPI.FunctionalTests.GenericTests;
using BlogCoreAPI.FunctionalTests.Helpers;
using BlogCoreAPI.Models.DTOs.Account;
using Xunit;

namespace BlogCoreAPI.FunctionalTests.Controllers
{
    [Collection("WebApplicationFactory")]
    public sealed class UsersController : AGenericTests<GetAccountDto, AddAccountDto, UpdateAccountDto>
    {
        protected override IEntityHelper<GetAccountDto, AddAccountDto, UpdateAccountDto> Helper { get; set; }
        public override async Task<GetAccountDto> AddRandomEntity()
        {
            var user = new AddAccountDto()
            {
                Email = Guid.NewGuid().ToString("N") + "@user.com",
                Password = "0a1234A@",
                UserDescription = "My description",
                UserName = Guid.NewGuid().ToString()[..20],
                ProfilePictureUrl = "https://www.facebook.com/images/fb_icon_325x325.png"
            };
            return await Helper.AddEntity(user);
        }

        public UsersController(TestWebApplicationFactory factory) : base(factory)
        {
            Helper = new UserHelper(Client);
        }
    }
}
