using System.Collections.Generic;
using System.Threading.Tasks;
using DbAccess.Data.POCO;
using DbAccess.Specifications;
using DbAccess.Specifications.SortSpecification;
using MyBlogAPI.DTO.Category;
using MyBlogAPI.Responses;

namespace MyBlogAPI.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<IEnumerable<GetCategoryDto>> GetAllCategories();

        Task<IEnumerable<GetCategoryDto>> GetCategories(FilterSpecification<Category> filter = null, int offset = 0, int limit = 1, SortingDirectionSpecification sort = SortingDirectionSpecification.Ascending, OrderBySpecification<Category> orderBy = null);

        Task<GetCategoryDto> GetCategory(int id);

        Task<GetCategoryDto> AddCategory(AddCategoryDto category);

        Task UpdateCategory(UpdateCategoryDto category);

        Task DeleteCategory(int id);
    }
}
