using System.Collections.Generic;

namespace MyBlogAPI.DTO
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
