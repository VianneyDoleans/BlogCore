using System;

namespace DBAccess.Contracts
{
    public interface IHasCreationDate
    {
        public DateTime PublishedAt { get; set; }
    }
}
