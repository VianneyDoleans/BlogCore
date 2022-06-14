
using DBAccess.Data;

namespace BlogCoreAPI.DTOs.Role
{
    /// <summary>
    /// Interface of <see cref="Role"/> Dto containing all the common properties of Role Dto Type (GET, ADD, UPDATE).
    /// </summary>
    public interface IRoleDto
    {
        public string Name { get; set; }
    }
}
