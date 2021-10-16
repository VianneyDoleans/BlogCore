using System.Collections.Generic;

namespace MyBlogAPI.DTO.Post
{
    /// <summary>
    /// UPDATE Dto type of <see cref="DbAccess.Data.POCO.Post"/>.
    /// </summary>
    public class UpdatePostDto : ADto, IPostDto
    {
        public string Content { get; set; }

        public string Name { get; set; }

        public int Author { get; set; }

        public int Category { get; set; }

        public virtual IEnumerable<int> Tags { get; set; }
    }
}
