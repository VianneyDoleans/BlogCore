using DBAccess.Data.POCO;

namespace BlogCoreAPI.DTOs.Role
{
    /// <summary>
    /// UPDATE Dto type of <see cref="Role"/>.
    /// </summary>
    public class UpdateRoleDto : ADto, IRoleDto
    {
        public string Name { get; set; }
    }
}
