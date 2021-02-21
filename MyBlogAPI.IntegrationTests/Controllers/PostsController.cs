using MyBlogAPI.DTO.Post;
using MyBlogAPI.IntegrationTests.GenericTests;
using MyBlogAPI.IntegrationTests.Helpers;
using Xunit;

namespace MyBlogAPI.IntegrationTests.Controllers
{
    public sealed class PostsController : AGenericTests<GetPostDto, AddPostDto, UpdatePostDto>, IClassFixture<TestWebApplicationFactory>
    {
        protected override IEntityHelper<GetPostDto, AddPostDto, UpdatePostDto> Helper { get; set; }

        public PostsController(TestWebApplicationFactory factory) : base(factory)
        {
            Helper = new PostHelper(Client);
        }
    }
}
