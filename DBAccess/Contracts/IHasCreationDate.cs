using System;

namespace DBAccess.Contracts
{
    public interface IHasCreationDate
    {
        public DateTimeOffset PublishedAt { get; set; }
    }
}
