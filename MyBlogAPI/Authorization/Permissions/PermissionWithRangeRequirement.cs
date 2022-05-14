using DbAccess.Data.POCO.Permission;
using Microsoft.AspNetCore.Authorization;
using MyBlogAPI.Authorization.Attributes;

namespace MyBlogAPI.Authorization.Permissions
{
    public class PermissionWithRangeRequirement : IAuthorizationRequirement
    {
        public PermissionAction Permission { get; }
        public PermissionTarget PermissionTarget { get; }
        public PermissionRange PermissionRange { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionWithPermissionRangeAllRequiredAttribute"/> class.
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
