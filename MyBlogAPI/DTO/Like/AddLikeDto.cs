using System;
using DbAccess.Data.POCO;
using MyBlogAPI.DTO.Comment;
using MyBlogAPI.DTO.Post;
using MyBlogAPI.DTO.User;

namespace MyBlogAPI.DTO.Like
{
    public class AddLikeDto
    {
        public int Id { get; set; }

        public DateTime PublishedAt { get; set; }

        public LikeableType LikeableType { get; set; }

        public int? Comment { get; set; }

        public int? Post { get; set; }

        public int User { get; set; }
    }
}
