using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using DBAccess.Data;
using DBAccess.Data.Permission;
using Microsoft.AspNetCore.Identity;

namespace DBAccess.Builders
{
    public class RoleBuilder
    {
        private readonly string _name;
        private readonly List<Permission> _permissions;
        private readonly RoleManager<Role> _roleManager;

        public RoleBuilder(string name, RoleManager<Role> roleManager)
        {
            _name = name;
            _roleManager = roleManager;
            _permissions = new List<Permission>();
        }

        public RoleBuilder WithCanReadAllOnAllResourcesExceptAccount()
        {
            WithCanReadAll(PermissionTarget.Category);
            WithCanReadAll(PermissionTarget.Comment);
            WithCanReadAll(PermissionTarget.Like);
            WithCanReadAll(PermissionTarget.Post);
            WithCanReadAll(PermissionTarget.Role);
            WithCanReadAll(PermissionTarget.Tag);
            WithCanReadAll(PermissionTarget.User);
            WithCanReadAll(PermissionTarget.Permission);
            return this;
        }

        public RoleBuilder WithCanUpdateAllOnAllResources()
        {
            WithCanUpdateAll(PermissionTarget.Category);
            WithCanUpdateAll(PermissionTarget.Comment);
            WithCanUpdateAll(PermissionTarget.Like);
            WithCanUpdateAll(PermissionTarget.Post);
            WithCanUpdateAll(PermissionTarget.Role);
            WithCanUpdateAll(PermissionTarget.Tag);
            WithCanUpdateAll(PermissionTarget.Permission);
            WithCanUpdateAll(PermissionTarget.Account);
            return this;
        }

        public RoleBuilder WithCanCreateAllOnAllResources()
        {
            WithCanCreateAll(PermissionTarget.Category);
            WithCanCreateAll(PermissionTarget.Comment);
            WithCanCreateAll(PermissionTarget.Like);
            WithCanCreateAll(PermissionTarget.Post);
            WithCanCreateAll(PermissionTarget.Role);
            WithCanCreateAll(PermissionTarget.Tag);
            WithCanCreateAll(PermissionTarget.Permission);
            WithCanCreateAll(PermissionTarget.Account);
            return this;
        }

        public RoleBuilder WithCanDeleteAllOnAllResources()
        {
            WithCanDeleteAll(PermissionTarget.Category);
            WithCanDeleteAll(PermissionTarget.Comment);
            WithCanDeleteAll(PermissionTarget.Like);
            WithCanDeleteAll(PermissionTarget.Post);
            WithCanDeleteAll(PermissionTarget.Role);
            WithCanDeleteAll(PermissionTarget.Tag);
            WithCanDeleteAll(PermissionTarget.Permission);
            WithCanDeleteAll(PermissionTarget.Account);
            return this;
        }

        public RoleBuilder WithCanReadAll(PermissionTarget target)
        {
            _permissions.Add(new Permission
            {
                PermissionAction = PermissionAction.CanRead,
                PermissionTarget = target,
                PermissionRange = PermissionRange.All
            });
            return this;
        }

        public RoleBuilder WithCanUpdateOwn(PermissionTarget target)
        {
            _permissions.Add(new Permission
            {
                PermissionAction = PermissionAction.CanUpdate,
                PermissionTarget = target,
                PermissionRange = PermissionRange.Own
            });
            return this;
        }

        public RoleBuilder WithCanDeleteOwn(PermissionTarget target)
        {
            _permissions.Add(new Permission
            {
                PermissionAction = PermissionAction.CanDelete,
                PermissionTarget = target,
                PermissionRange = PermissionRange.Own
            });
            return this;
        }

        public RoleBuilder WithCanReadOwn(PermissionTarget target)
        {
            _permissions.Add(new Permission
            {
                PermissionAction = PermissionAction.CanRead,
                PermissionTarget = target,
                PermissionRange = PermissionRange.Own
            });
            return this;
        }

        public RoleBuilder WithCanCreateOwn(PermissionTarget target)
        {
            _permissions.Add(new Permission
            {
                PermissionAction = PermissionAction.CanCreate,
                PermissionTarget = target,
                PermissionRange = PermissionRange.Own
            });
            return this;
        }


        public RoleBuilder WithCanUpdateAll(PermissionTarget target)
        {
            _permissions.Add(new Permission
            {
                PermissionAction = PermissionAction.CanUpdate,
                PermissionTarget = target,
                PermissionRange = PermissionRange.All
            });
            return this;
        }

        public RoleBuilder WithCanDeleteAll(PermissionTarget target)
        {
            _permissions.Add(new Permission
            {
                PermissionAction = PermissionAction.CanDelete,
                PermissionTarget = target,
                PermissionRange = PermissionRange.All
            });
            return this;
        }

        public RoleBuilder WithCanCreateAll(PermissionTarget target)
        {
            _permissions.Add(new Permission
            {
                PermissionAction = PermissionAction.CanCreate,
                PermissionTarget = target,
                PermissionRange = PermissionRange.All
            });
            return this;
        }

        public async Task<Role> Build()
        {
            var role = new Role() { Name = _name };
            await _roleManager.CreateAsync(role);
            foreach (var permission in _permissions)
            {
                await _roleManager.AddClaimAsync(role,
                    new Claim("Permission", JsonSerializer.Serialize(permission)));
            }
            return role;
        }
    }
}
