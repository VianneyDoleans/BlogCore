using System;
using System.Threading.Tasks;
using DbAccess.Data.POCO;
using MyBlogAPI.DTO.Category;
using MyBlogAPI.DTO.Like;
using MyBlogAPI.DTO.Post;
using MyBlogAPI.DTO.User;
using MyBlogAPI.FunctionalTests.GenericTests;
using MyBlogAPI.FunctionalTests.Helpers;
using Xunit;

namespace MyBlogAPI.FunctionalTests.Controllers
{
    public sealed class LikesController : AGenericTests<GetLikeDto, AddLikeDto, UpdateLikeDto>, IClassFixture<TestWebApplicationFactory>
    {
        protected override IEntityHelper<GetLikeDto, AddLikeDto, UpdateLikeDto> Helper { get; set; }

        private readonly UserHelper _userHelper;
        private readonly CategoryHelper _categoryHelper;
        private readonly PostHelper _postHelper;
        public override async Task<GetLikeDto> AddRandomEntity()
        {
            var user = new AddUserDto()
            {
                Email = Guid.NewGuid() + "@user.com",
                Password = "abcdh",
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
            var like = await Helper.AddEntity(new AddLikeDto()
            {
                Post = (await _postHelper.AddEntity(post)).Id, 
                LikeableType = LikeableType.Post,
                User = userId
            });
            return like;
        }

        public LikesController(TestWebApplicationFactory factory) : base(factory)
        {
            Helper = new LikeHelper(Client);
            _userHelper = new UserHelper(Client);
            _categoryHelper = new CategoryHelper(Client);
            _postHelper = new PostHelper(Client);
        }
    }
}
