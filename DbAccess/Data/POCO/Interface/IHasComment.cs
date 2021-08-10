using System;
using System.Collections.Generic;
using System.Text;

namespace DbAccess.Data.POCO.Interface
{
    public interface IHasComment
    {
        public Comment Comment { get; set; }
    }
}
