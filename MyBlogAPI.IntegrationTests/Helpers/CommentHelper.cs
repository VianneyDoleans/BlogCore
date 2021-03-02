using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using MyBlogAPI.DTO.Comment;

namespace MyBlogAPI.IntegrationTests.Helpers
{
    public class CommentHelper : AEntityHelper<GetCommentDto, AddCommentDto, UpdateCommentDto>
    {
        public CommentHelper(HttpClient client, string baseUrl = "/comments") : base(baseUrl, client)
        {
        }

        public override bool Equals(GetCommentDto first, GetCommentDto second)
        {
            if (first == null || second == null)
                return false;
            if (first.Likes == null && second.Likes != null ||
                first.Likes != null && second.Likes == null)
                return false;
            if (first.Likes != null && second.Likes != null)
                return first.Likes.SequenceEqual(second.Likes) &&
                       first.Author == second.Author &&
                       first.PostParent == second.PostParent &&
                       first.CommentParent == second.CommentParent &&
                       first.Content == second.Content;
            return first.Author == second.Author &&
                   first.PostParent == second.PostParent &&
                   first.CommentParent == second.CommentParent &&
                   first.Content == second.Content;

        }

        protected override UpdateCommentDto ModifyTUpdate(UpdateCommentDto entity)
        {
            return new UpdateCommentDto
            {
                Id = entity.Id, Content = Guid.NewGuid().ToString(),
                Author = entity.Author,
                CommentParent = entity.CommentParent,
                PostParent = entity.PostParent
            };
        }
    }
}
