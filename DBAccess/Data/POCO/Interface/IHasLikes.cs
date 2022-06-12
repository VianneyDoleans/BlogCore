using System.Collections.Generic;

namespace DBAccess.Data.POCO.Interface
{
    public interface IHasLikes
    {
        public ICollection<Like> Likes { get; set; }
    }
}
