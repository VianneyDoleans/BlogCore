using DBAccess.Data;

namespace DBAccess.Contracts
{
    public interface IHasPost
    {
        public Post Post { get; set; }
    }
}
