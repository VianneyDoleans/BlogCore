using System;

namespace DBAccess.Data.POCO.Interface
{
    public interface IHasLastLogin
    {
        public DateTime LastLogin { get; set; }
    }
}
