using System;
using System.Collections.Generic;

namespace BlogCoreAPI.Models.DTOs.Comment
{
    /// <summary>
    /// GET Dto type of <see cref="Comment"/>.
    /// </summary>
    public class GetCommentDto : ADto, ICommentDto
    {
        public int Author { get; set; }

        public int PostParent { get; set; }

        public int? CommentParent { get; set; }

        public DateTimeOffset PublishedAt { get; set; }

        public DateTimeOffset? ModifiedAt { get; set; }

        public string Content { get; set; }

        public IEnumerable<int> Likes { get; set; }

        public IEnumerable<int> ChildrenComments { get; set; }
    }
}
