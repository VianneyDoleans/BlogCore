using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbAccess.Data.POCO;
using MyBlogAPI.DTO.Comment;
using MyBlogAPI.DTO.Post;
using MyBlogAPI.DTO.User;

namespace MyBlogAPI.DTO.Like
{
    public class GetLikeDto
    {
        public int Id { get; set; }

        public DateTime PublishedAt { get; set; }

        public LikeableType LikeableType { get; set; }

        public int? Comment { get; set; }

        public int? Post { get; set; }

        public int User { get; set; }
    }
}
