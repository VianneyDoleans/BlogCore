using System;

namespace MyBlogAPI.Models
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PermissionRequiredAttribute : Attribute
    {
        private Permission Permission { get; }

        public PermissionRequiredAttribute(Permission permission)
        {
                Permission = permission;
        }
    }
}
