
using MyBlogAPI.DTO.Contracts;

namespace MyBlogAPI.DTO.Comment
{
    /// <summary>
    /// UPDATE Dto type of <see cref="DbAccess.Data.POCO.Comment"/>.
    /// </summary>
    public class UpdateCommentDto : ADto, ICommentDto
    {
        public int Author { get; set; }

        public int PostParent { get; set; }

        public int? CommentParent { get; set; }

        public string Content { get; set; }
    }
}
