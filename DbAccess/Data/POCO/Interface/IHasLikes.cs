using System;
using System.Collections.Generic;
using System.Text;

namespace DbAccess.Data.POCO.Interface
{
    public interface IHasLikes
    {
        public ICollection<Like> Likes { get; set; }
    }
}
