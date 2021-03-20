using System.Collections.Generic;

namespace MyBlogAPI.DTO.Role
{
    public class GetRoleDto : ADto, IRoleDto
    {
        public string Name { get; set; }

        public virtual IEnumerable<int> Users { get; set; }
    }
}
