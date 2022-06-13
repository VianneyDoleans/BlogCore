using System.Collections.Generic;
using DBAccess.Data.JoiningEntity;

namespace DBAccess.Contracts
{
    public interface IHasPostTag
    {
        public ICollection<PostTag> PostTags { get; set; }
    }
}
