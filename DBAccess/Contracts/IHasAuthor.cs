
using DBAccess.Data;

namespace DBAccess.Contracts
{
    public interface IHasAuthor
    {
        public User Author { get; set; }
    }
}
