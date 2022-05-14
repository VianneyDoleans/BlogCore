
namespace MyBlogAPI.DTO.Category
{
    /// <summary>
    /// Add Dto type of <see cref="DbAccess.Data.POCO.Category"/>.
    /// </summary>
    public class AddCategoryDto : ICategoryDto
    {
        public string Name { get; set; }
    }
}
