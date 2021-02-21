using MyBlogAPI.DTO.Category;
using MyBlogAPI.IntegrationTests.GenericTests;
using MyBlogAPI.IntegrationTests.Helpers;
using Xunit;

namespace MyBlogAPI.IntegrationTests.Controllers
{
    public sealed class CategoriesController : AGenericTests<GetCategoryDto, AddCategoryDto, UpdateCategoryDto>, IClassFixture<TestWebApplicationFactory>
    {
        protected override IEntityHelper<GetCategoryDto, AddCategoryDto, UpdateCategoryDto> Helper { get; set; }

        public CategoriesController(TestWebApplicationFactory factory) : base(factory)
        {
            Helper = new CategoryHelper(Client);
        }
    }
}
