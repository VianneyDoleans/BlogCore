using System;
using System.Threading.Tasks;
using BlogCoreAPI.DTOs.Tag;
using BlogCoreAPI.FunctionalTests.GenericTests;
using BlogCoreAPI.FunctionalTests.Helpers;
using Xunit;

namespace BlogCoreAPI.FunctionalTests.Controllers
{
    public sealed class TagsController : AGenericTests<GetTagDto, AddTagDto, UpdateTagDto>, IClassFixture<TestWebApplicationFactory>
    {
        protected override IEntityHelper<GetTagDto, AddTagDto, UpdateTagDto> Helper { get; set; }
        public override async Task<GetTagDto> AddRandomEntity()
        {
            var tag = new AddTagDto()
            {
                Name = Guid.NewGuid().ToString("N")
            };
            return await Helper.AddEntity(tag);
        }

        public TagsController(TestWebApplicationFactory factory) : base(factory)
        {
            Helper = new TagHelper(Client);
        }
    }
}
