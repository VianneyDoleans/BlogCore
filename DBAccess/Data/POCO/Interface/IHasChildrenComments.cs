using System.Collections.Generic;

namespace DBAccess.Data.POCO.Interface
{
    public interface IHasChildrenComments
    { 
        public ICollection<Comment> ChildrenComments { get; set; }
    }
}
