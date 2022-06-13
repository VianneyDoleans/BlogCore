using DBAccess.Data;

namespace BlogCoreAPI.DTOs.Category
{
    /// <summary>
    /// UPDATE Dto type of <see cref="Category"/>.
    /// </summary>
    public class UpdateCategoryDto : ADto, ICategoryDto
    {
        public string Name { get; set; }
    }
}
