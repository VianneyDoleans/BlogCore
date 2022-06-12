using System;

namespace DBAccess.Data.POCO.Interface
{
    public interface IHasRegisteredAt
    {
        public DateTime RegisteredAt { get; set; }
    }
}
