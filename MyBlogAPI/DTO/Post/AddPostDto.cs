using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Configuration.Conventions;
using MyBlogAPI.DTO.Category;
using MyBlogAPI.DTO.Comment;
using MyBlogAPI.DTO.Like;
using MyBlogAPI.DTO.Tag;
using MyBlogAPI.DTO.User;

namespace MyBlogAPI.DTO.Post
{
    public class AddPostDto : IDto
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string Name { get; set; }

        public int Author { get; set; }

        public int Category { get; set; }

        public DateTime PublishedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public virtual ICollection<int> Tags { get; set; }
    }
}
