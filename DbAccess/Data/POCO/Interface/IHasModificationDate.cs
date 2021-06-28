using System;

namespace DbAccess.Data.POCO.Interface
{
    public interface IHasModificationDate
    {
        public DateTime? ModifiedAt { get; set; }
    }
}
