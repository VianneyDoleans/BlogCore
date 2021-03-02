using System;
using System.Threading.Tasks;
using MyBlogAPI.DTO.Category;
using MyBlogAPI.DTO.User;
using MyBlogAPI.IntegrationTests.GenericTests;
using MyBlogAPI.IntegrationTests.Helpers;
using Xunit;

namespace MyBlogAPI.IntegrationTests.Controllers
{
    public sealed class CategoriesController : AGenericTests<GetCategoryDto, AddCategoryDto, UpdateCategoryDto>, IClassFixture<TestWebApplicationFactory>
    {
        protected override IEntityHelper<GetCategoryDto, AddCategoryDto, UpdateCategoryDto> Helper { get; set; }
        public override async Task<GetCategoryDto> AddRandomEntity()
        {
            var category = new AddCategoryDto()
            {
                Name = Guid.NewGuid().ToString("N")
            };
            return await Helper.AddEntity(category);
        }

        public CategoriesController(TestWebApplicationFactory factory) : base(factory)
        {
            Helper = new CategoryHelper(Client);
        }
    }
}
