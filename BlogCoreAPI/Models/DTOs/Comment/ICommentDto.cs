using BlogCoreAPI.DTOs.Contracts;

namespace BlogCoreAPI.DTOs.Comment
{
    /// <summary>
    /// Interface of <see cref="Comment"/> Dto containing all the common properties of Comment Dto Type (GET, ADD, UPDATE).
    /// </summary>
    public interface ICommentDto : IHasAuthor
    {
        public new int Author { get; set; }

        public int PostParent { get; set; }

        public int? CommentParent { get; set; }

        public string Content { get; set; }
    }
}
