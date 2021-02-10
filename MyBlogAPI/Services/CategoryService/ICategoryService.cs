using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBlogAPI.DTO;

namespace MyBlogAPI.Services.CategoryService
{
    public interface ICategoryService
    {
        ICollection<Category> GetAllCategories();

        Category GetCategory(int id);

        void AddCategory(Category category);

        void UpdateCategory(Category category);

        void DeleteCategory(int id);
    }
}
