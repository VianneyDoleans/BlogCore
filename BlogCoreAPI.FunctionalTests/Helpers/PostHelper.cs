using System;
using System.Linq;
using System.Net.Http;
using BlogCoreAPI.DTOs.Post;

namespace BlogCoreAPI.FunctionalTests.Helpers
{
    public class PostHelper : AEntityHelper<GetPostDto, AddPostDto, UpdatePostDto>
    {
        public PostHelper(HttpClient client, string baseUrl = "/posts") : base(baseUrl, client)
        {
        }

        public override bool Equals(GetPostDto first, GetPostDto second)
        {
            if (first == null || second == null)
                return false;
            if (first.Tags == null && second.Tags != null ||
                first.Tags != null && second.Tags == null)
                return false;
            if (first.Tags != null && second.Tags != null)
                return first.Tags.SequenceEqual(second.Tags) &&
                       first.Name == second.Name &&
                       first.Category == second.Category &&
                       first.Author == second.Author &&
                       first.Content == second.Content;
            return first.Name == second.Name &&
                   first.Category == second.Category &&
                   first.Author == second.Author &&
                   first.Content == second.Content;
        }

        protected override UpdatePostDto ModifyTUpdate(UpdatePostDto entity)
        {
            return new UpdatePostDto
            {
                Id = entity.Id,
                Name = Guid.NewGuid().ToString(),
                Author = entity.Author,
                Category = entity.Category,
                Content = entity.Content,
                Tags = entity.Tags
            };
        }
    }
}
