namespace DbAccess.Data.Models.Permission
{
    /// <summary>
    /// Permission enum allowing to define a permission action for a specific resource.
    /// </summary>
    /// <example>
    /// User can read post, comment, like...
    /// Writer can do the same, but also create/update post, tag, etc.
    /// </example>
    public enum PermissionAction
    {
        CanRead = 0,
        CanDelete = 1,
        CanCreate = 2,
        CanUpdate = 4
    }
}
