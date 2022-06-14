using System.Collections.Generic;
using DBAccess.Data.JoiningEntity;

namespace DBAccess.Contracts
{
    public interface IHasUserRoles
    {
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
