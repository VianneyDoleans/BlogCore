using System.Collections.Generic;
using DBAccess.Data.POCO.JoiningEntity;

namespace DBAccess.Data.POCO.Interface
{
    public interface IHasUserRoles
    {
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
