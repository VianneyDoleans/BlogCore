using System;
using System.Threading.Tasks;
using BlogCoreAPI.FunctionalTests.GenericTests;
using BlogCoreAPI.FunctionalTests.Helpers;
using BlogCoreAPI.Models.DTOs.Category;
using BlogCoreAPI.Models.DTOs.Comment;
using BlogCoreAPI.Models.DTOs.Post;
using BlogCoreAPI.Models.DTOs.User;
using Xunit;

namespace BlogCoreAPI.FunctionalTests.Controllers
{
    public sealed class CommentsController : AGenericTests<GetCommentDto, AddCommentDto, UpdateCommentDto>,
        IClassFixture<TestWebApplicationFactory>
    {
        protected override IEntityHelper<GetCommentDto, AddCommentDto, UpdateCommentDto> Helper { get; set; }
        private readonly PostHelper _postHelper;
        private readonly UserHelper _userHelper;
        private readonly CategoryHelper _categoryHelper;

        public override async Task<GetCommentDto> AddRandomEntity()
        {
            var user = new AddUserDto()
            {
                Email = Guid.NewGuid().ToString("N") + "@user.com",
                Password = "0a1234A@",
                UserDescription = "My description",
                UserName = Guid.NewGuid().ToString("N")[..20]
            };
            var userId = (await _userHelper.AddEntity(user)).Id;
            var category = new AddCategoryDto()
            {
                Name = Guid.NewGuid().ToString("N")
            };
            var post = new AddPostDto()
            {
                Name = Guid.NewGuid().ToString("N"),
                Author = userId,
                Category = (await _categoryHelper.AddEntity(category)).Id,
                Content = "test POstDto"
            };
            var comment = new AddCommentDto()
            {
                Author = userId,
                PostParent = (await _postHelper.AddEntity(post)).Id,
                Content = "test CommentDto"
            };
            return await Helper.AddEntity(comment);
        }

        public CommentsController(TestWebApplicationFactory factory) : base(factory)
        {
            Helper = new CommentHelper(Client);
            _postHelper = new PostHelper(Client);
            _userHelper = new UserHelper(Client);
            _categoryHelper = new CategoryHelper(Client);
        }
    }
}
