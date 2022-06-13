using System.Collections.Generic;
using DBAccess.Data;

namespace DBAccess.Contracts
{
    public interface IHasPosts
    {
        public ICollection<Post> Posts { get; set; }
    }
}
