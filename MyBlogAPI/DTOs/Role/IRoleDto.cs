
namespace MyBlogAPI.DTOs.Role
{
    /// <summary>
    /// Interface of <see cref="DbAccess.Data.POCO.Role"/> Dto containing all the common properties of Role Dto Type (GET, ADD, UPDATE).
    /// </summary>
    public interface IRoleDto
    {
        public string Name { get; set; }
    }
}
