using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBlogAPI.DTO;

namespace MyBlogAPI.Services.RoleService
{
    public interface IRoleService
    {
        ICollection<Role> GetAllRoles();

        Role GetRole(int id);

        void AddRole(Role role);

        void UpdateRole(Role role);

        void DeleteRole(int id);
    }
}
