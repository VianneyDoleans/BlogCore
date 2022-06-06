using DbAccess.Data.POCO.Permission;
using Microsoft.AspNetCore.Authorization;
using MyBlogAPI.Authorization.Attributes;

namespace MyBlogAPI.Authorization.Permissions
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionAction Permission { get; }
        public PermissionTarget PermissionTarget { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionWithPermissionRangeAllRequiredAttribute"/> class.
        /// </summary>
        /// <param name="permission"></param>
        /// <param name="permissionTarget"></param>
        public PermissionRequirement(PermissionAction permission, PermissionTarget permissionTarget)
        {
            Permission = permission;
            PermissionTarget = permissionTarget;
        }
    }
}
