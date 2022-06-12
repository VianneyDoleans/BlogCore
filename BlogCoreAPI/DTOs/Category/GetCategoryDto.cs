using System.Collections.Generic;

namespace MyBlogAPI.DTOs.Category
{
    /// <summary>
    /// GET Dto type of <see cref="DbAccess.Data.POCO.Category"/>.
    /// </summary>
    public class GetCategoryDto : ADto, ICategoryDto
    {
        public string Name { get; set; }

        public virtual IEnumerable<int> Posts { get; set; }
    }
}
