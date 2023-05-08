using System;

namespace DBAccess.Contracts
{
    public interface IHasRegisteredAt
    {
        public DateTimeOffset RegisteredAt { get; set; }
    }
}
