
using DBAccess.Data.POCO;

namespace BlogCoreAPI.DTOs.Comment
{
    /// <summary>
    /// Add Dto type of <see cref="Comment"/>.
    /// </summary>
    public class AddCommentDto : ICommentDto
    {
        public int Author { get; set; }

        public int PostParent { get; set; }

        public int? CommentParent { get; set; }

        public string Content { get; set; }
    }
}
