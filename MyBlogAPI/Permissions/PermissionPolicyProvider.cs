using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using MyBlogAPI.Attributes;

namespace MyBlogAPI.Permissions
{
    public class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        private readonly AuthorizationOptions _options;

        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
        {
            _options = options.Value;
        }

        // Dynamically creates a policy with a requirement that contains the permission.
        // The policy name must match the permission that is needed.
        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            return await base.GetPolicyAsync(policyName)
                   ?? new AuthorizationPolicyBuilder()
                       .AddRequirements(new PermissionRequirement(DbAccess.Data.POCO.Permission.PermissionAction.CanCreate, DbAccess.Data.POCO.Permission.PermissionTarget.Category))
                       .Build();
        }
    }
}
