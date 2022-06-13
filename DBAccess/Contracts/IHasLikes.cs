using System.Collections.Generic;
using DBAccess.Data;

namespace DBAccess.Contracts
{
    public interface IHasLikes
    {
        public ICollection<Like> Likes { get; set; }
    }
}
