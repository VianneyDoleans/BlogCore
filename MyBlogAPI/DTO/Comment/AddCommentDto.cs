
using MyBlogAPI.DTO.Contracts;

namespace MyBlogAPI.DTO.Comment
{
    /// <summary>
    /// Add Dto type of <see cref="DbAccess.Data.POCO.Comment"/>.
    /// </summary>
    public class AddCommentDto : ICommentDto
    {
        public int Author { get; set; }

        public int PostParent { get; set; }

        public int? CommentParent { get; set; }

        public string Content { get; set; }
    }
}
