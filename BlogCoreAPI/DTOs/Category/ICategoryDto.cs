using DBAccess.Data;

namespace BlogCoreAPI.DTOs.Category
{
    /// <summary>
    /// Interface of <see cref="Category"/> Dto containing all the common properties of Category Dto Type (GET, ADD, UPDATE).
    /// </summary>
    public interface ICategoryDto
    {
        public string Name { get; set; }
    }
}
