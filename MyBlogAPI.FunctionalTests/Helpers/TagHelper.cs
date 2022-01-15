using System;
using System.Net.Http;
using MyBlogAPI.DTO.Tag;

namespace MyBlogAPI.FunctionalTests.Helpers
{
    public class TagHelper : AEntityHelper<GetTagDto, AddTagDto, UpdateTagDto>
    {
        public TagHelper(HttpClient client, string baseUrl = "/tags") : base(baseUrl, client)
        {
        }

        public override bool Equals(GetTagDto first, GetTagDto second)
        {
            if (first == null || second == null)
                return false;
            return first.Name == second.Name;
        }

        protected override UpdateTagDto ModifyTUpdate(UpdateTagDto entity)
        {
            return new UpdateTagDto { Id = entity.Id, Name = Guid.NewGuid().ToString("N") };
        }
    }
}
