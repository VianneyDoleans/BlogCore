using System;

namespace DBAccess.Contracts
{
    public interface IHasRegisteredAt
    {
        public DateTime RegisteredAt { get; set; }
    }
}
