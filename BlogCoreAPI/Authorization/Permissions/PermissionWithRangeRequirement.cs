using DbAccess.Data.POCO.Permission;
using Microsoft.AspNetCore.Authorization;

namespace MyBlogAPI.Authorization.Permissions
{
    /// <summary>
    /// Same as <see cref="PermissionRequirement"/> but with <see cref="PermissionRange"/> in addition (own or all resources).
    /// </summary>
    public class PermissionWithRangeRequirement : IAuthorizationRequirement
    {
        public PermissionAction Permission { get; }
        public PermissionTarget PermissionTarget { get; }
        public PermissionRange PermissionRange { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionWithRangeRequirement"/> class.
        /// </summary>
        /// <param name="permission"></param>
        /// <param name="permissionTarget"></param>
        /// <param name="permissionRange"></param>
        public PermissionWithRangeRequirement(PermissionAction permission, PermissionTarget permissionTarget, PermissionRange permissionRange)
        {
            Permission = permission;
            PermissionTarget = permissionTarget;
            PermissionRange = permissionRange;
        }
    }
}
