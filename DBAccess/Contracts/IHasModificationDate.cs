using System;

namespace DBAccess.Contracts
{
    public interface IHasModificationDate
    {
        public DateTime? ModifiedAt { get; set; }
    }
}
