using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBlogAPI.DTO.Comment;
using MyBlogAPI.DTO.Like;
using MyBlogAPI.DTO.Post;
using MyBlogAPI.DTO.Role;

namespace MyBlogAPI.DTO.User
{
    public class GetUserDto : IDto
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string EmailAddress { get; set; }

        public DateTime RegisteredAt { get; set; }

        public DateTime LastLogin { get; set; }

        public string UserDescription { get; set; }

        public virtual IEnumerable<int> Roles { get; set; }

        /*public virtual IEnumerable<int> Posts { get; set; }

        public virtual IEnumerable<int> Comments { get; set; }

        public virtual IEnumerable<int> Likes { get; set; }*/
    }
}
