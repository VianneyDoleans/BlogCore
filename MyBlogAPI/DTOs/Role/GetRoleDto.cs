using System.Collections.Generic;

namespace MyBlogAPI.DTOs.Role
{
    /// <summary>
    /// GET Dto type of <see cref="DbAccess.Data.POCO.Role"/>.
    /// </summary>
    public class GetRoleDto : ADto, IRoleDto
    {
        public string Name { get; set; }

        public virtual IEnumerable<int> Users { get; set; }
    }
}
