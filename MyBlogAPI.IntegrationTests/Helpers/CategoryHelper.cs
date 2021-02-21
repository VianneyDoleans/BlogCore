using System;
using System.Linq;
using System.Net.Http;
using MyBlogAPI.DTO.Category;

namespace MyBlogAPI.IntegrationTests.Helpers
{
    public class CategoryHelper : AEntityHelper<GetCategoryDto, AddCategoryDto, UpdateCategoryDto>
    {
        public CategoryHelper(HttpClient client, string baseUrl = "/categories") : base(baseUrl, client)
        {
        }

        protected override AddCategoryDto CreateTAdd()
        {
            var user = new AddCategoryDto()
            {
                Name = Guid.NewGuid().ToString()
            };
            return user;
        }

        public override bool Equals(GetCategoryDto first, GetCategoryDto second)
        {
            if (first == null || second == null)
                return false;
            return first.Name == second.Name &&
                   first.Posts.SequenceEqual(second.Posts);
        }

        protected override UpdateCategoryDto ModifyTUpdate(UpdateCategoryDto entity)
        {
            return new UpdateCategoryDto {Id = entity.Id, Name = Guid.NewGuid().ToString()};
        }
    }
}
