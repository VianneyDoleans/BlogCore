using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using MyBlogAPI.DTO.Category;
using MyBlogAPI.DTO.User;

namespace MyBlogAPI.IntegrationTests.Helpers
{
    public class CategoryHelper : GenericEntityHelper<GetCategoryDto, AddCategoryDto>
    {
        public CategoryHelper(HttpClient client, string baseUrl = "/categories") : base(baseUrl, client)
        {
        }

        protected override AddCategoryDto CreateAddEntity()
        {
            var user = new AddCategoryDto()
            {
                Name = Guid.NewGuid().ToString()
            };
            return user;
        }
    }
}
