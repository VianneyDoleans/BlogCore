using System;

namespace DBAccess.Contracts
{
    public interface IHasModificationDate
    {
        public DateTimeOffset? ModifiedAt { get; set; }
    }
}
