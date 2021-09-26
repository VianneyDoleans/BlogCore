using System;

namespace DbAccess.Data.POCO.Interface
{
    public interface IHasLastLogin
    {
        public DateTime LastLogin { get; set; }
    }
}
