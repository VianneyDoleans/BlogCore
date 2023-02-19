using System.Collections.Generic;
using System.Threading.Tasks;
using BlogCoreAPI.Models.DTOs.Category;
using DBAccess.Data;
using DBAccess.Specifications;
using DBAccess.Specifications.FilterSpecifications;
using DBAccess.Specifications.SortSpecification;

namespace BlogCoreAPI.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<IEnumerable<GetCategoryDto>> GetAllCategories();

        public Task<IEnumerable<GetCategoryDto>> GetCategories(FilterSpecification<Category> filterSpecification = null,
            PagingSpecification pagingSpecification = null,
            SortSpecification<Category> sortSpecification = null);

        public Task<int> CountCategoriesWhere(FilterSpecification<Category> filterSpecification = null);

        Task<GetCategoryDto> GetCategory(int id);

        Task<GetCategoryDto> AddCategory(AddCategoryDto category);

        Task UpdateCategory(UpdateCategoryDto category);

        Task DeleteCategory(int id);
    }
}
