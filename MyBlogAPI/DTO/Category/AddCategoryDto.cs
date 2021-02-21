using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBlogAPI.DTO.Post;

namespace MyBlogAPI.DTO.Category
{
    public class AddCategoryDto : IDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
