using System;
using DbAccess.Data.POCO.Permission;

namespace MyBlogAPI.Attributes
{
    /// <summary>
    ///  Attribute allowing to define the corresponding permission needed for an resource endpoint.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class PermissionRequiredAttribute : Attribute
    {
        private PermissionAction Permission { get; }
        private PermissionTarget PermissionTarget { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionRequiredAttribute"/> class.
        /// </summary>
        /// <param name="permission"></param>
        /// <param name="permissionTarget"></param>
        public PermissionRequiredAttribute(PermissionAction permission, PermissionTarget permissionTarget)
        {
                Permission = permission;
                PermissionTarget = permissionTarget;
        }
    }
}
