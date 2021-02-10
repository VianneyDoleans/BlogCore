using System;

namespace MyBlogAPI.DTO
{
    public enum LikeableType : byte
    {
        Comment = 0,
        Post = 1,
    }

    public class Like
    {

        public int Id { get; set; }

        public DateTime PublishedAt { get; set; }

        public LikeableType LikeableType { get; set; }

        public Comment Comment { get; set; }

        public Post Post { get; set; }

        public DbAccess.Data.POCO.User User { get; set; }
    }
}
