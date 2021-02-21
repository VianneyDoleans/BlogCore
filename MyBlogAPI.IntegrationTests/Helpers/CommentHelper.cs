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

        protected override AddCommentDto CreateTAdd()
        {
            var comment = new AddCommentDto()
            {
                Author = 1,
                PostParent = 1,
                Content = "test CommentDto"
            };
            return comment;
        }

        public override bool Equals(GetCommentDto first, GetCommentDto second)
        {
            if (first == null || second == null)
                return false;
            return first.Author == second.Author &&
                   first.PostParent == second.PostParent &&
                   first.CommentParent == second.CommentParent &&
                   first.Content == second.Content &&
                   first.Likes.SequenceEqual(second.Likes);
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
