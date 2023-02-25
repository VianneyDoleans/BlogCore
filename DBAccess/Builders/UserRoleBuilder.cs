using System.Linq;
using DBAccess.Data;
using DBAccess.Data.JoiningEntity;
using DBAccess.DataContext;

namespace DBAccess.Builders
{
    public class UserRoleBuilder
    {
        private readonly BlogCoreContext _context;
        private Role _role;
        private User _user;

        public UserRoleBuilder(BlogCoreContext context)
        {
            _context = context;
        }

        public UserRoleBuilder WithUser(string userName)
        {
            _user = _context.Users.Single(x => x.UserName == userName);
            return this;
        }

        public UserRoleBuilder WithRole(string roleName)
        {
            _role = _context.Roles.Single(x => x.Name == roleName);
            return this;
        }

        public UserRole Build()
        {
            return new UserRole() { Role = _role, User = _user };
        }
    }
}
