using System;
using System.Collections.Generic;
using System.Text;

namespace DbAccess.Data.POCO.Interface
{
    public interface IHasPosts
    {
        public ICollection<Post> Posts { get; set; }
    }
}
