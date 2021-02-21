using MyBlogAPI.DTO.Like;
using MyBlogAPI.IntegrationTests.GenericTests;
using MyBlogAPI.IntegrationTests.Helpers;
using Xunit;

namespace MyBlogAPI.IntegrationTests.Controllers
{
    public sealed class LikesController : AGenericTests<GetLikeDto, AddLikeDto, UpdateLikeDto>, IClassFixture<TestWebApplicationFactory>
    {
        protected override IEntityHelper<GetLikeDto, AddLikeDto, UpdateLikeDto> Helper { get; set; }

        public LikesController(TestWebApplicationFactory factory) : base(factory)
        {
            Helper = new LikeHelper(Client);
        }
    }
}
