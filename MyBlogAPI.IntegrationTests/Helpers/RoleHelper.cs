using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using MyBlogAPI.DTO.Role;

namespace MyBlogAPI.IntegrationTests.Helpers
{
    public class RoleHelper : AEntityHelper<GetRoleDto, AddRoleDto, UpdateRoleDto>
    {
        public RoleHelper(HttpClient client, string baseUrl = "/roles") : base(baseUrl, client)
        {
        }

        protected override AddRoleDto CreateTAdd()
        {
            var user = new AddRoleDto()
            {
                Name = Guid.NewGuid().ToString()
            };
            return user;
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

        protected override UpdateRoleDto ModifyTUpdate(UpdateRoleDto entity)
        {
            return new UpdateRoleDto { Id = entity.Id, Name = Guid.NewGuid().ToString() };
        }
    }
}
