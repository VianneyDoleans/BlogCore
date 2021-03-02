using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlogAPI.DTO.User
{
    public class UpdateUserDto : ADto
    {
        public string Username { get; set; }

        public string EmailAddress { get; set; }

        public string Password { get; set; }

        public string UserDescription { get; set; }
    }
}
