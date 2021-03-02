using System;
using DbAccess.Data.POCO;
using MyBlogAPI.DTO.Comment;
using MyBlogAPI.DTO.Post;
using MyBlogAPI.DTO.User;

namespace MyBlogAPI.DTO.Like
{
    public class AddLikeDto
    {
        public LikeableType LikeableType { get; set; }

        public int? Comment { get; set; }

        public int? Post { get; set; }

        public int User { get; set; }
    }
}
