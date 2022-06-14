using DBAccess.Data.Permission;
using Microsoft.AspNetCore.Authorization;

namespace BlogCoreAPI.Authorization.Permissions
{
    /// <summary>
    /// Permission requirement used to describe an action (can read, can write, can delete, ...) (<see cref="PermissionAction"/>)
    ///  on a resource (category, comment, like, ...) (<see cref="PermissionTarget"/>)
    /// </summary>
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionAction Permission { get; }
        public PermissionTarget PermissionTarget { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionRequirement"/> class.
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
