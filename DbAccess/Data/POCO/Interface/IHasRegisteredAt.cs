using System;

namespace DbAccess.Data.POCO.Interface
{
    public interface IHasRegisteredAt
    {
        public DateTime RegisteredAt { get; set; }
    }
}
