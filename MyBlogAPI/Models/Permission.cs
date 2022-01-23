using DbAccess.Data.POCO;

namespace MyBlogAPI.Models
{
    /// <summary>
    /// Permission enum allowing to define the permission of a <see cref="Role"/> for a specific resource.
    /// </summary>
    /// <example>
    /// User can read post, comment, like...
    /// Writer can do the same, but also create/update post, tag, etc.
    /// </example>
    public enum Permission
    {
        CanRead,
        CanDelete,
        CanCreate,
        CanUpdate
    }
}
