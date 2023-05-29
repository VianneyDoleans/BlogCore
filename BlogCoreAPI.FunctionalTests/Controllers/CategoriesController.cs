using System;
using System.Threading.Tasks;
using BlogCoreAPI.FunctionalTests.GenericTests;
using BlogCoreAPI.FunctionalTests.Helpers;
using BlogCoreAPI.Models.DTOs.Category;
using Xunit;

namespace BlogCoreAPI.FunctionalTests.Controllers
{
    [Collection("WebApplicationFactory")]
    public sealed class CategoriesController : AGenericTests<GetCategoryDto, AddCategoryDto, UpdateCategoryDto>
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
