using System.Collections.Generic;
using DBAccess.Data.POCO.JoiningEntity;

namespace DBAccess.Data.POCO.Interface
{
    public interface IHasPostTag
    {
        public ICollection<PostTag> PostTags { get; set; }
    }
}
