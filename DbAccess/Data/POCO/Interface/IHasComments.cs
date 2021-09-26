using System.Collections.Generic;

namespace DbAccess.Data.POCO.Interface
{
    public interface IHasComments
    {
        public ICollection<Comment> Comments { get; set; }
    }
}
