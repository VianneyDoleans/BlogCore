using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using MyBlogAPI.DTO.Tag;

namespace MyBlogAPI.IntegrationTests.Helpers
{
    public class TagHelper : AEntityHelper<GetTagDto, AddTagDto, UpdateTagDto>
    {
        public TagHelper(HttpClient client, string baseUrl = "/tags") : base(baseUrl, client)
        {
        }

        protected override AddTagDto CreateTAdd()
        {
            var user = new AddTagDto()
            {
                Name = Guid.NewGuid().ToString()
            };
            return user;
        }

        public override bool Equals(GetTagDto first, GetTagDto second)
        {
            if (first == null || second == null)
                return false;
            return first.Name == second.Name;
        }

        protected override UpdateTagDto ModifyTUpdate(UpdateTagDto entity)
        {
            return new UpdateTagDto { Id = entity.Id, Name = Guid.NewGuid().ToString() };
        }
    }
}
