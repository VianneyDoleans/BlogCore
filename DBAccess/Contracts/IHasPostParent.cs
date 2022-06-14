using DBAccess.Data;

namespace DBAccess.Contracts
{
    public interface IHasPostParent
    {
        public Post PostParent { get; set; }
    }
}
