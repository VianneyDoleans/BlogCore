using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlogAPI.DTO.User
{
    public class GetUserDto
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string EmailAddress { get; set; }

        public DateTime RegisteredAt { get; set; }

        public DateTime LastLogin { get; set; }

        public string UserDescription { get; set; }

        public virtual ICollection<Role> UserRoles { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Like> Likes { get; set; }
    }
}
