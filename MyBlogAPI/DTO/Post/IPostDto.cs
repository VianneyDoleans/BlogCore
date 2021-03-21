using System.Collections.Generic;

namespace MyBlogAPI.DTO.Post
{
    public interface IPostDto
    {
        public string Content { get; set; }

        public string Name { get; set; }

        public int Author { get; set; }

        public int Category { get; set; }

        public IEnumerable<int> Tags { get; set; }
    }
}
