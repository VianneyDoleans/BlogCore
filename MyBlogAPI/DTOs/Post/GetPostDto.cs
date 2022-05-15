using System;
using System.Collections.Generic;

namespace MyBlogAPI.DTOs.Post
{
    /// <summary>
    /// GET Dto type of <see cref="DbAccess.Data.POCO.Post"/>.
    /// </summary>
    public class GetPostDto : ADto, IPostDto
    {
        public string Content { get; set; }

        public string Name { get; set; }

        public int Author { get; set; }

        public int Category { get; set; }

        public DateTime PublishedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public IEnumerable<int> Likes { get; set; }

        public virtual IEnumerable<int> Tags { get; set; }
    }
}
