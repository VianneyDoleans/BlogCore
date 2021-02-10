using System;
using System.Collections.Generic;

namespace MyBlogAPI.DTO
{
    public class Post
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string Name { get; set; }

        public DbAccess.Data.POCO.User Author { get; set; }

        public Category Category { get; set; }

        public DateTime PublishedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }

        public virtual ICollection<Like> Likes { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        /*public Post()
        {
            PostTags = new HashSet<PostTag>();
        }*/
    }
}
