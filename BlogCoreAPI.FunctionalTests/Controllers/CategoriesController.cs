using System;
using System.Threading.Tasks;
using BlogCoreAPI.DTOs.Category;
using BlogCoreAPI.FunctionalTests.GenericTests;
using BlogCoreAPI.FunctionalTests.Helpers;
using Xunit;

namespace BlogCoreAPI.FunctionalTests.Controllers
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
