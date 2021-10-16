using System.Collections.Generic;
using DbAccess.Data.POCO.JoiningEntity;

namespace DbAccess.Data.POCO.Interface
{
    public interface IHasUserRoles
    {
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
