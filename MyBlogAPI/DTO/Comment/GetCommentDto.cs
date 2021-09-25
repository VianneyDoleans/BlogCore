using System;
using System.Collections.Generic;

namespace MyBlogAPI.DTO.Comment
{
    public class GetCommentDto : ADto, ICommentDto
    {
        public int Author { get; set; }

        public int PostParent { get; set; }

        public int? CommentParent { get; set; }

        public DateTime PublishedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public string Content { get; set; }

        public IEnumerable<int> Likes { get; set; }

        public IEnumerable<int> ChildrenComments { get; set; }
    }
}
