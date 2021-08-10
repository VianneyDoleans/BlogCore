using System;
using System.Collections.Generic;
using System.Text;

namespace DbAccess.Data.POCO.Interface
{
    public interface IHasPostParent
    {
        public Post PostParent { get; set; }
    }
}
