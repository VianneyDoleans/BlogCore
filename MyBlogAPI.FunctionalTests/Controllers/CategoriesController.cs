using System;
using System.Threading.Tasks;
using MyBlogAPI.DTO.Category;
using MyBlogAPI.FunctionalTests.GenericTests;
using MyBlogAPI.FunctionalTests.Helpers;
using Xunit;

namespace MyBlogAPI.FunctionalTests.Controllers
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
