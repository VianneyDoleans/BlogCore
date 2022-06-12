using System;

namespace DBAccess.Data.POCO.Interface
{
    public interface IHasModificationDate
    {
        public DateTime? ModifiedAt { get; set; }
    }
}
