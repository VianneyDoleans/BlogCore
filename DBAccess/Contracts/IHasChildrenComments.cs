using System.Collections.Generic;
using DBAccess.Data;

namespace DBAccess.Contracts
{
    public interface IHasChildrenComments
    { 
        public ICollection<Comment> ChildrenComments { get; set; }
    }
}
