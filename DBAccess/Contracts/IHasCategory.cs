using DBAccess.Data;

namespace DBAccess.Contracts
{
    public interface IHasCategory
    {
        public Category Category { get; set; }
    }
}
