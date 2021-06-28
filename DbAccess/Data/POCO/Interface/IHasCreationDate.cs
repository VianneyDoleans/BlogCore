using System;

namespace DbAccess.Data.POCO.Interface
{
    public interface IHasCreationDate
    {
        public DateTime PublishedAt { get; set; }
    }
}
