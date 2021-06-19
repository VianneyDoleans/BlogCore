using System.Collections.Generic;
using System.Threading.Tasks;
using MyBlogAPI.DTO.Category;
using MyBlogAPI.Responses;

namespace MyBlogAPI.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<IEnumerable<GetCategoryDto>> GetAllCategories();

        Task<IEnumerable<GetCategoryDto>> GetCategories(int offset = 0, int limit = 0);

        Task<GetCategoryDto> GetCategory(int id);

        Task<GetCategoryDto> AddCategory(AddCategoryDto category);

        Task UpdateCategory(UpdateCategoryDto category);

        Task DeleteCategory(int id);
    }
}
