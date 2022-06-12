using System.Collections.Generic;

namespace DBAccess.Data.POCO.Interface
{
    public interface IHasComments
    {
        public ICollection<Comment> Comments { get; set; }
    }
}
