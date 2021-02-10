using System;
using System.Collections.Generic;

namespace MyBlogAPI.DTO
{
    public class Comment
    {
        public int Id { get; set; }

        public DbAccess.Data.POCO.User Author { get; set; }

        public Post PostParent { get; set; }

        public Comment CommentParent { get; set; }

        public DateTime PublishedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public string Content { get; set; }

        public ICollection<Like> Likes { get; set; }
    }
}
