using System;
using System.Threading.Tasks;
using BlogCoreAPI.FunctionalTests.GenericTests;
using BlogCoreAPI.FunctionalTests.Helpers;
using BlogCoreAPI.Models.DTOs.Account;
using BlogCoreAPI.Models.DTOs.Category;
using BlogCoreAPI.Models.DTOs.Post;
using BlogCoreAPI.Models.DTOs.User;
using Xunit;

namespace BlogCoreAPI.FunctionalTests.Controllers
{
    public sealed class PostsController : AGenericTests<GetPostDto, AddPostDto, UpdatePostDto>, IClassFixture<TestWebApplicationFactory>
    {
        protected override IEntityHelper<GetPostDto, AddPostDto, UpdatePostDto> Helper { get; set; }

        private readonly UserHelper _userHelper;
        private readonly CategoryHelper _categoryHelper;

        public override async Task<GetPostDto> AddRandomEntity()
        {
            var user = new AddAccountDto()
            {
                Email = Guid.NewGuid() + "@user.com",
                Password = "0a1234A@",
                UserDescription = "My description",
                UserName = Guid.NewGuid().ToString("N")[..20]
            };
            var category = new AddCategoryDto()
            {
                Name = Guid.NewGuid().ToString("N")
            };
            var userAdded = await _userHelper.AddEntity(user);
            var categoryAdded = await _categoryHelper.AddEntity(category);
            var post = new AddPostDto()
            {
                Name = Guid.NewGuid().ToString("N"),
                Author = userAdded.Id,
                Category = categoryAdded.Id,
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
