using System;
using System.Linq;
using System.Net.Http;
using BlogCoreAPI.Models.DTOs.Comment;

namespace BlogCoreAPI.FunctionalTests.Helpers
{
    public class CommentHelper : AEntityHelper<GetCommentDto, AddCommentDto, UpdateCommentDto>
    {
        public CommentHelper(HttpClient client, string baseUrl = "/comments") : base(baseUrl, client)
        {
        }

        public override bool Equals(UpdateCommentDto first, GetCommentDto second)
        {
            if (first == null || second == null)
                return false;
            return first.Author == second.Author &&
                   first.PostParent == second.PostParent &&
                   first.CommentParent == second.CommentParent &&
                   first.Content == second.Content;
        }

        public override bool Equals(UpdateCommentDto first, UpdateCommentDto second)
        {
            if (first == null || second == null)
                return false;
            return first.Author == second.Author &&
                   first.PostParent == second.PostParent &&
                   first.CommentParent == second.CommentParent &&
                   first.Content == second.Content;
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

        public override UpdateCommentDto GenerateTUpdate(int id, GetCommentDto entity)
        {
            return new UpdateCommentDto
            {
                Id = id, Content = Guid.NewGuid().ToString(),
                Author = entity.Author,
                CommentParent = entity.CommentParent,
                PostParent = entity.PostParent
            };
        }
    }
}
