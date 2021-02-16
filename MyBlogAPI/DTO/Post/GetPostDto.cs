using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBlogAPI.DTO.Category;
using MyBlogAPI.DTO.Comment;
using MyBlogAPI.DTO.Like;
using MyBlogAPI.DTO.Tag;
using MyBlogAPI.DTO.User;

namespace MyBlogAPI.DTO.Post
{
    public class GetPostDto
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string Name { get; set; }

        public int Author { get; set; }

        public int Category { get; set; }

        public DateTime PublishedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public virtual IEnumerable<int> Tags { get; set; }

        /*public virtual IEnumerable<int> Likes { get; set; }

        public virtual IEnumerable<int> Comments { get; set; }*/
    }
}
