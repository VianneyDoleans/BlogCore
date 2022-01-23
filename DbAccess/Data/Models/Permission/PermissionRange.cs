namespace DbAccess.Data.Models.Permission
{
    /// <summary>
    /// Enum complementary to <see cref="PermissionAction"/> allowing to define the range of the permission.
    /// </summary>
    /// <example>
    /// Writer can create/update own resources.
    /// Admin can create/update all resources (not only own).
    /// </example>
    public enum PermissionRange
    {
        /// <summary>
        /// the permission is valid only for own resources
        /// </summary>
        Own = 0,
        /// <summary>
        /// the permission is valid for all the resources
        /// </summary>
        All = 1
    }
}
