﻿using System;

namespace DBAccess.Data.Permission
{
    /// <summary>
    /// Permission enum allowing to define a permission for a <see cref="Role"/>.
    /// </summary>
    /// <example>
    /// User can read own post, create own like.
    /// Writer can also create/update own post.
    /// Admin can update all resource (not only own).
    /// Etc.
    /// </example>
    public sealed class Permission : IEquatable<Permission>
    {
        public  PermissionAction PermissionAction { get; set; }

        public PermissionTarget PermissionTarget { get; set; }

        public PermissionRange PermissionRange { get; set; }

        public bool Equals(Permission other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return PermissionAction == other.PermissionAction && PermissionTarget == other.PermissionTarget && PermissionRange == other.PermissionRange;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Permission)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int)PermissionAction, (int)PermissionTarget, (int)PermissionRange);
        }
    }
}
