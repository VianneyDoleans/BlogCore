using System;

namespace DBAccess.Contracts
{
    public interface IHasLastLogin
    {
        public DateTime LastLogin { get; set; }
    }
}
