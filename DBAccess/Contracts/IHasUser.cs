
using DBAccess.Data;

namespace DBAccess.Contracts
{
    public interface IHasUser
    {
        public User User { get; set; }
    }
}
