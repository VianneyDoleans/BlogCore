namespace MyBlogAPI.DTOs.Role
{
    /// <summary>
    /// Add Dto type of <see cref="DbAccess.Data.POCO.Role"/>.
    /// </summary>
    public class AddRoleDto : IRoleDto
    {
        public string Name { get; set; }

    }
}
