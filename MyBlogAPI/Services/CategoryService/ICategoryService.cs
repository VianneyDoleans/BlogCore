using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBlogAPI.DTO;
using MyBlogAPI.DTO.Category;

namespace MyBlogAPI.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<IEnumerable<GetCategoryDto>> GetAllCategories();

        Task<GetCategoryDto> GetCategory(int id);

        Task AddCategory(AddCategoryDto category);

        Task UpdateCategory(AddCategoryDto category);

        Task DeleteCategory(int id);
    }
}
