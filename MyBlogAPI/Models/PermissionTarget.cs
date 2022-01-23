namespace MyBlogAPI.Models
{
    /// <summary>
    /// Enum complementary to <see cref="Permission"/> allowing to define the target of the permission.
    /// </summary>
    /// <example>
    /// Writer can create/update own resources.
    /// Admin can create/update all resources (not only own).
    /// </example>
    public enum PermissionTargetEnum
    {
        /// <summary>
        /// the permission is valid only for own resources
        /// </summary>
        Own,
        /// <summary>
        /// the permission is valid for all the resources
        /// </summary>
        All
    }
}
