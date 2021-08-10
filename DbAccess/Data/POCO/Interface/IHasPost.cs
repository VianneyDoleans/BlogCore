using System;
using System.Collections.Generic;
using System.Text;

namespace DbAccess.Data.POCO.Interface
{
    public interface IHasPost
    {
        public Post Post { get; set; }
    }
}
