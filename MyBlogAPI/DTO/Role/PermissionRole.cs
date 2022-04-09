using DbAccess.Data.POCO.Permission;

namespace MyBlogAPI.DTO.Role
{
    public class PermissionRole
    {
        public string RoleId { get; set; }

        public Permission Permission { get; set; }
    }
}
