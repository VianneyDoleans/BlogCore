namespace DbAccess.Data.Models.Permission
{
    /// <summary>
    /// Permission enum allowing to define a permission for a <see cref="POCO.Role"/>.
    /// </summary>
    /// <example>
    /// User can read own post, create own like.
    /// Writer can also create/update own post.
    /// Admin can update all resource (not only own).
    /// Etc.
    /// </example>
    public class Permission
    {
        public  PermissionAction PermissionAction { get; set; }

        public PermissionTarget PermissionTarget { get; set; }

        public PermissionRange PermissionRange { get; set; }
    }
}
