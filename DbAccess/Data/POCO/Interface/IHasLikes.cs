using System.Collections.Generic;

namespace DbAccess.Data.POCO.Interface
{
    public interface IHasLikes
    {
        public ICollection<Like> Likes { get; set; }
    }
}
