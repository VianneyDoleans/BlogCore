using System.Collections.Generic;
using MyBlogAPI.DTO.Post;

namespace MyBlogAPI.DTO.Tag
{
    public class GetTagDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        /*public virtual IEnumerable<int> Posts { get; set; }*/
    }
}
