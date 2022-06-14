using DBAccess.Data;

namespace DBAccess.Contracts
{
    public interface IHasComment
    {
        public Comment Comment { get; set; }
    }
}
