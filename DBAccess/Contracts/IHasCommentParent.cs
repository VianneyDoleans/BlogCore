using DBAccess.Data;

namespace DBAccess.Contracts
{
    public interface IHasCommentParent
    {
        public Comment CommentParent { get; set; }
    }
}
