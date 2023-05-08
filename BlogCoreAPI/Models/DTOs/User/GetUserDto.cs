using System.Collections.Generic;
using System;

namespace BlogCoreAPI.Models.DTOs.User
{
    public class GetUserDto : ADto
    {
        public string UserName { get; set; }
        
        public string ProfilePictureUrl { get; set; }

        public DateTimeOffset RegisteredAt { get; set; }

        public DateTimeOffset LastLogin { get; set; }

        public string UserDescription { get; set; }

        public virtual IEnumerable<int> Roles { get; set; }
    }
}
