using System.Collections.Generic;

namespace DbAccess.Data.POCO.Interface
{
    public interface IHasPosts
    {
        public ICollection<Post> Posts { get; set; }
    }
}
