namespace BlogCoreAPI.DTOs.Category
{
    /// <summary>
    /// Add Dto type of <see cref="Category"/>.
    /// </summary>
    public class AddCategoryDto : ICategoryDto
    {
        public string Name { get; set; }
    }
}
