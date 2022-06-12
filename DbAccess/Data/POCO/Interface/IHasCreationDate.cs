using System;

namespace DBAccess.Data.POCO.Interface
{
    public interface IHasCreationDate
    {
        public DateTime PublishedAt { get; set; }
    }
}
