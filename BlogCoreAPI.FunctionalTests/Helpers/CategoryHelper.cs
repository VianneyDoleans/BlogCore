using System;
using System.Linq;
using System.Net.Http;
using BlogCoreAPI.Models.DTOs.Category;

namespace BlogCoreAPI.FunctionalTests.Helpers
{
    public class CategoryHelper : AEntityHelper<GetCategoryDto, AddCategoryDto, UpdateCategoryDto>
    {
        public CategoryHelper(HttpClient client, string baseUrl = "/categories") : base(baseUrl, client)
        {
        }

        public override bool Equals(UpdateCategoryDto first, GetCategoryDto second)
        {
            if (first == null || second == null)
                return false;
            return first.Name == second.Name;
        }

        public override bool Equals(UpdateCategoryDto first, UpdateCategoryDto second)
        {
            if (first == null || second == null)
                return false;
            return first.Name == second.Name;
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

        public override UpdateCategoryDto GenerateTUpdate(int id, GetCategoryDto entity)
        {
            return new UpdateCategoryDto {Id = id, Name = Guid.NewGuid().ToString("N") };
        }
    }
}
