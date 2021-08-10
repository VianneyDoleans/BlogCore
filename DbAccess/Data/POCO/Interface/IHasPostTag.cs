using System;
using System.Collections.Generic;
using System.Text;
using DbAccess.Data.POCO.JoiningEntity;

namespace DbAccess.Data.POCO.Interface
{
    public interface IHasPostTag
    {
        public ICollection<PostTag> PostTags { get; set; }
    }
}
