using System.Collections.Generic;

namespace MyBlogAPI.DTO.Role
{
    public class GetRoleDto : ADto
    {
        public string Name { get; set; }

        public virtual IEnumerable<int> Users { get; set; }
    }
}
