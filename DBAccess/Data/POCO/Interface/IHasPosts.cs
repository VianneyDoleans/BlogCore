using System.Collections.Generic;

namespace DBAccess.Data.POCO.Interface
{
    public interface IHasPosts
    {
        public ICollection<Post> Posts { get; set; }
    }
}
