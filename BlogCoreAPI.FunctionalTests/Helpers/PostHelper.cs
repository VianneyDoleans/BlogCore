using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using BlogCoreAPI.Models.DTOs.Post;

namespace BlogCoreAPI.FunctionalTests.Helpers
{
    public class PostHelper : AEntityHelper<GetPostDto, AddPostDto, UpdatePostDto>
    {
        public PostHelper(HttpClient client, string baseUrl = "/posts") : base(baseUrl, client)
        {
        }

        public override bool Equals(UpdatePostDto first, GetPostDto second)
        {
            if (first == null || second == null)
                return false;
            var firstIsEmpty = first.Tags == null || !first.Tags.Any();
            var secondIsEmpty = second.Tags == null || !second.Tags.Any();
            if ((firstIsEmpty && !secondIsEmpty) ||
                (!firstIsEmpty && secondIsEmpty))
                return false;
            if (first.Tags != null && second.Tags != null)
                return first.Tags.SequenceEqual(second.Tags) &&
                       first.Name == second.Name &&
                       first.Category == second.Category &&
                       first.Author == second.Author &&
                       first.Content == second.Content &&
                       first.ThumbnailUrl == second.ThumbnailUrl;
            return first.Name == second.Name &&
                   first.Category == second.Category &&
                   first.Author == second.Author &&
                   first.Content == second.Content && 
                   first.ThumbnailUrl == second.ThumbnailUrl;
        }

        public override bool Equals(UpdatePostDto first, UpdatePostDto second)
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
                       first.Content == second.Content &&
                       first.ThumbnailUrl == second.ThumbnailUrl;
            return first.Name == second.Name &&
                   first.Category == second.Category &&
                   first.Author == second.Author &&
                   first.Content == second.Content && 
                   first.ThumbnailUrl == second.ThumbnailUrl;
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
                       first.Content == second.Content &&
                       first.ThumbnailUrl == second.ThumbnailUrl;
            return first.Name == second.Name &&
                   first.Category == second.Category &&
                   first.Author == second.Author &&
                   first.Content == second.Content && 
                   first.ThumbnailUrl == second.ThumbnailUrl;
        }

        public override UpdatePostDto GenerateTUpdate(int id, GetPostDto entity)
        {
            return new UpdatePostDto
            {
                Id = id,
                Name = Guid.NewGuid().ToString(),
                Author = entity.Author,
                Category = entity.Category - 1,
                Content = entity.Content + "v2",
                // Tags = new List<int>() { 1, 2, 3 },
                ThumbnailUrl = "https://www.google.com/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png"
            };
        }
    }
}
