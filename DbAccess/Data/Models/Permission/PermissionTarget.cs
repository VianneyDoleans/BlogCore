
namespace DbAccess.Data.Models.Permission
{
    /// <summary>
    /// Enum defining the target of a <see cref="PermissionAction"/>
    /// </summary>
    public enum PermissionTarget
    {
        Category = 0,
        Comment = 1,
        Like = 2,
        Post = 3,
        Role = 4,
        Tag = 5,
        User = 6
    }
}
