using System;
using System.Collections.Generic;
using System.Text;

namespace DbAccess.Data.POCO.Interface
{
    public interface IHasCategory
    {
        public Category Category { get; set; }
    }
}
