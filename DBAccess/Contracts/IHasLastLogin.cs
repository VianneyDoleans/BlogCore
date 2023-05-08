using System;

namespace DBAccess.Contracts
{
    public interface IHasLastLogin
    {
        public DateTimeOffset LastLogin { get; set; }
    }
}
