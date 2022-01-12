﻿namespace MyBlogAPI.DTO.Comment
{
    /// <summary>
    /// Interface of <see cref="DbAccess.Data.POCO.Comment"/> Dto containing all the common properties of Comment Dto Type (GET, ADD, UPDATE).
    /// </summary>
    public interface ICommentDto
    {
        public int Author { get; set; }

        public int PostParent { get; set; }

        public int? CommentParent { get; set; }

        public string Content { get; set; }
    }
}