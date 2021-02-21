using MyBlogAPI.DTO.Role;
using MyBlogAPI.IntegrationTests.GenericTests;
using MyBlogAPI.IntegrationTests.Helpers;
using Xunit;

namespace MyBlogAPI.IntegrationTests.Controllers
{
    public sealed class RolesController : AGenericTests<GetRoleDto, AddRoleDto, UpdateRoleDto>, IClassFixture<TestWebApplicationFactory>
    {
        protected override IEntityHelper<GetRoleDto, AddRoleDto, UpdateRoleDto> Helper { get; set; }

        public RolesController(TestWebApplicationFactory factory) : base(factory)
        {
            Helper = new RoleHelper(Client);
        }
    }
}
