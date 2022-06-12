using DBAccess.Data.POCO;

namespace BlogCoreAPI.DTOs.Role
{
    /// <summary>
    /// Add Dto type of <see cref="Role"/>.
    /// </summary>
    public class AddRoleDto : IRoleDto
    {
        public string Name { get; set; }

    }
}
