using MyBlogAPI.DTO.Tag;
using MyBlogAPI.IntegrationTests.GenericTests;
using MyBlogAPI.IntegrationTests.Helpers;
using Xunit;

namespace MyBlogAPI.IntegrationTests.Controllers
{
    public sealed class TagsController : AGenericTests<GetTagDto, AddTagDto, UpdateTagDto>, IClassFixture<TestWebApplicationFactory>
    {
        protected override IEntityHelper<GetTagDto, AddTagDto, UpdateTagDto> Helper { get; set; }

        public TagsController(TestWebApplicationFactory factory) : base(factory)
        {
            Helper = new TagHelper(Client);
        }
    }
}
