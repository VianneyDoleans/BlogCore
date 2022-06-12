using System;
using System.Threading.Tasks;
using DbAccess.Data.POCO.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using MyBlogAPI.Authorization.Permissions;

namespace MyBlogAPI.Authorization
{
    /// <summary>
    /// 
    /// </summary>
    public class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        /// <inheritdoc />
        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
        {
        }

        private static PermissionWithRangeRequirement GetPermissionWithRangeRequirement(string policyName)
        {
            if (policyName.StartsWith("permission."))
            {
                var values = policyName.Split('.');
                if (values.Length == 4)
                {
                    var actionSuccess = Enum.TryParse(values[1], out PermissionAction permissionAction); 
                    var targetSuccess = Enum.TryParse(values[2], out PermissionTarget permissionTarget);
                    var rangeSuccess = Enum.TryParse(values[3], out PermissionRange permissionRange);
                    if (!actionSuccess || !targetSuccess || !rangeSuccess)
                        return null;
                    var permissionRequirement = new PermissionWithRangeRequirement(permissionAction, permissionTarget, permissionRange);
                    return permissionRequirement;
                }
            }
            return null;
        }

        /// <inheritdoc />
        public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            return await base.GetPolicyAsync(policyName)
                   ?? new AuthorizationPolicyBuilder()
                       .AddRequirements(GetPermissionWithRangeRequirement(policyName)).Build();
        }
    }
}
