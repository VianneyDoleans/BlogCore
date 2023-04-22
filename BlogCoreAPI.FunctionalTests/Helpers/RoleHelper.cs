using System;
using System.Linq;
using System.Net.Http;
using BlogCoreAPI.Models.DTOs.Role;

namespace BlogCoreAPI.FunctionalTests.Helpers
{
    public class RoleHelper : AEntityHelper<GetRoleDto, AddRoleDto, UpdateRoleDto>
    {
        public RoleHelper(HttpClient client, string baseUrl = "/roles") : base(baseUrl, client)
        {
        }

        public override bool Equals(UpdateRoleDto first, GetRoleDto second)
        {
            if (first == null || second == null)
                return false;
            return first.Name == second.Name;
        }

        public override bool Equals(UpdateRoleDto first, UpdateRoleDto second)
        {
            if (first == null || second == null)
                return false;
            return first.Name == second.Name;
        }

        public override bool Equals(GetRoleDto first, GetRoleDto second)
        {
            if (first == null || second == null)
                return false;
            if (first.Users == null && second.Users != null ||
                first.Users != null && second.Users == null)
                return false;
            if (first.Users != null && second.Users != null)
                return first.Users.SequenceEqual(second.Users) &&
                       first.Name == second.Name;
            return first.Name == second.Name;

        }

        public override UpdateRoleDto GenerateTUpdate(int id, GetRoleDto entity)
        {
            return new UpdateRoleDto { Id = id, Name = Guid.NewGuid().ToString("N")[..20] };
        }
    }
}
