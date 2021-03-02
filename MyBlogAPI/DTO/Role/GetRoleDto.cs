using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBlogAPI.DTO.User;

namespace MyBlogAPI.DTO.Role
{
    public class GetRoleDto : ADto
    {
        public string Name { get; set; }

        public virtual IEnumerable<int> Users { get; set; }
    }
}
