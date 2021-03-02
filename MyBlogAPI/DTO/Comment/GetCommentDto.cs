using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Configuration.Conventions;
using MyBlogAPI.DTO.Like;
using MyBlogAPI.DTO.Post;
using MyBlogAPI.DTO.User;

namespace MyBlogAPI.DTO.Comment
{
    public class GetCommentDto : ADto
    {
        public int Author { get; set; }

        public int PostParent { get; set; }

        public int? CommentParent { get; set; }

        public DateTime PublishedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public string Content { get; set; }

        public IEnumerable<int> Likes { get; set; }
    }
}
