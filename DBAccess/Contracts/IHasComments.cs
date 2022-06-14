using System.Collections.Generic;
using DBAccess.Data;

namespace DBAccess.Contracts
{
    public interface IHasComments
    {
        public ICollection<Comment> Comments { get; set; }
    }
}
