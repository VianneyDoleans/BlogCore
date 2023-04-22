using System;
using System.Net.Http;
using BlogCoreAPI.Models.DTOs.Tag;

namespace BlogCoreAPI.FunctionalTests.Helpers
{
    public class TagHelper : AEntityHelper<GetTagDto, AddTagDto, UpdateTagDto>
    {
        public TagHelper(HttpClient client, string baseUrl = "/tags") : base(baseUrl, client)
        {
        }

        public override bool Equals(UpdateTagDto first, GetTagDto second)
        {
            if (first == null || second == null)
                return false;
            return first.Name == second.Name;
        }

        public override bool Equals(UpdateTagDto first, UpdateTagDto second)
        {
            if (first == null || second == null)
                return false;
            return first.Name == second.Name;
        }

        public override bool Equals(GetTagDto first, GetTagDto second)
        {
            if (first == null || second == null)
                return false;
            return first.Name == second.Name;
        }

        public override UpdateTagDto GenerateTUpdate(int id, GetTagDto entity)
        {
            return new UpdateTagDto { Id = id, Name = Guid.NewGuid().ToString("N") };
        }
    }
}
