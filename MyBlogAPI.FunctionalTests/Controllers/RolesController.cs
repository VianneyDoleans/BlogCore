using System;
using System.Threading.Tasks;
using MyBlogAPI.DTOs.Role;
using MyBlogAPI.FunctionalTests.GenericTests;
using MyBlogAPI.FunctionalTests.Helpers;
using Xunit;

namespace MyBlogAPI.FunctionalTests.Controllers
{
    public sealed class RolesController : AGenericTests<GetRoleDto, AddRoleDto, UpdateRoleDto>, IClassFixture<TestWebApplicationFactory>
    {
        protected override IEntityHelper<GetRoleDto, AddRoleDto, UpdateRoleDto> Helper { get; set; }
        public override async Task<GetRoleDto> AddRandomEntity()
        {
            var role = new AddRoleDto()
            {
                Name = Guid.NewGuid().ToString("N")[..20]
            };
            return await Helper.AddEntity(role);
        }

        public RolesController(TestWebApplicationFactory factory) : base(factory)
        {
            Helper = new RoleHelper(Client);
        }
    }
}
