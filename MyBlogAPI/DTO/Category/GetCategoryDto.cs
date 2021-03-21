using System.Collections.Generic;

namespace MyBlogAPI.DTO.Category
{
    public class GetCategoryDto : ADto, ICategoryDto
    {
        public string Name { get; set; }

        //[MapTo(nameof(DbAccess.Data.POCO.Post.Id))]
        public virtual IEnumerable<int> Posts { get; set; }
    }
}
