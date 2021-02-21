using MyBlogAPI.DTO.Comment;
using MyBlogAPI.IntegrationTests.GenericTests;
using MyBlogAPI.IntegrationTests.Helpers;
using Xunit;

namespace MyBlogAPI.IntegrationTests.Controllers
{
    public sealed class CommentsController : AGenericTests<GetCommentDto, AddCommentDto, UpdateCommentDto>, IClassFixture<TestWebApplicationFactory>
    {
    protected override IEntityHelper<GetCommentDto, AddCommentDto, UpdateCommentDto> Helper { get; set; }

    public CommentsController(TestWebApplicationFactory factory) : base(factory)
    {
        Helper = new CommentHelper(Client);
    }
    }
}
