using System;
using System.Threading.Tasks;
using MyBlogAPI.DTO.Category;
using MyBlogAPI.DTO.Post;
using MyBlogAPI.DTO.User;
using MyBlogAPI.FunctionalTests.GenericTests;
using MyBlogAPI.FunctionalTests.Helpers;
using Xunit;

namespace MyBlogAPI.FunctionalTests.Controllers
{
    public sealed class PostsController : AGenericTests<GetPostDto, AddPostDto, UpdatePostDto>, IClassFixture<TestWebApplicationFactory>
    {
        protected override IEntityHelper<GetPostDto, AddPostDto, UpdatePostDto> Helper { get; set; }

        private readonly UserHelper _userHelper;
        private readonly CategoryHelper _categoryHelper;

        public override async Task<GetPostDto> AddRandomEntity()
        {
            var user = new AddUserDto()
            {
                Email = Guid.NewGuid() + "@user.com",
                Password = "abcdh",
                UserDescription = "My description",
                UserName = Guid.NewGuid().ToString("N")[..20]
            };
            var category = new AddCategoryDto()
            {
                Name = Guid.NewGuid().ToString("N")
            };
            var post = new AddPostDto()
            {
                Name = Guid.NewGuid().ToString("N"),
                Author = (await _userHelper.AddEntity(user)).Id,
                Category = (await _categoryHelper.AddEntity(category)).Id,
                Content = "test POstDto"
            };
            return await Helper.AddEntity(post);
        }

        public PostsController(TestWebApplicationFactory factory) : base(factory)
        {
            Helper = new PostHelper(Client);
            _categoryHelper = new CategoryHelper(Client);
            _userHelper = new UserHelper(Client);
        }
    }
}
