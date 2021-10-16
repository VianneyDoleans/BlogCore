using System.Collections.Generic;

namespace DbAccess.Data.POCO.Interface
{
    public interface IHasChildrenComments
    { 
        public ICollection<Comment> ChildrenComments { get; set; }
    }
}
