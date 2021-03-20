using System;
using System.Collections.Generic;

namespace MyBlogAPI.DTO.User
{
    public class GetUserDto : ADto
    {
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
