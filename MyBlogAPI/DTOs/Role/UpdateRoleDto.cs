namespace MyBlogAPI.DTOs.Role
{
    /// <summary>
    /// UPDATE Dto type of <see cref="DbAccess.Data.POCO.Role"/>.
    /// </summary>
    public class UpdateRoleDto : ADto, IRoleDto
    {
        public string Name { get; set; }
    }
}
