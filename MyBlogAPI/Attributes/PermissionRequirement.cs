using DbAccess.Data.POCO.Permission;
using Microsoft.AspNetCore.Authorization;

namespace MyBlogAPI.Attributes
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionAction Permission { get; }
        public PermissionTarget PermissionTarget { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionRequiredAttribute"/> class.
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
