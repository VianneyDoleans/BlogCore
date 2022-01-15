using System;
using System.Linq;
using System.Net.Http;
using MyBlogAPI.DTO.Category;

namespace MyBlogAPI.FunctionalTests.Helpers
{
    public class CategoryHelper : AEntityHelper<GetCategoryDto, AddCategoryDto, UpdateCategoryDto>
    {
        public CategoryHelper(HttpClient client, string baseUrl = "/categories") : base(baseUrl, client)
        {
        }

        public override bool Equals(GetCategoryDto first, GetCategoryDto second)
        {
            if (first == null || second == null)
                return false;
            if (first.Posts == null && second.Posts != null ||
                first.Posts != null && second.Posts == null)
                return false;
            if (first.Posts != null && second.Posts != null)
                return first.Posts.SequenceEqual(second.Posts) && first.Name == second.Name;
            return first.Name == second.Name;
        }

        protected override UpdateCategoryDto ModifyTUpdate(UpdateCategoryDto entity)
        {
            return new UpdateCategoryDto {Id = entity.Id, Name = Guid.NewGuid().ToString("N") };
        }
    }
}
