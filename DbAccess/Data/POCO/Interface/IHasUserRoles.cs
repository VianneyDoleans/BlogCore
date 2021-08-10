using System;
using System.Collections.Generic;
using System.Text;
using DbAccess.Data.POCO.JoiningEntity;

namespace DbAccess.Data.POCO.Interface
{
    public interface IHasUserRoles
    {
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
