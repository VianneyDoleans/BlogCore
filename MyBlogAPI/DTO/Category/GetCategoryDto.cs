using System.Collections.Generic;
using AutoMapper.Configuration.Conventions;

namespace MyBlogAPI.DTO.Category
{
    public class GetCategoryDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [MapTo(nameof(DbAccess.Data.POCO.Post.Id))]
        public virtual IEnumerable<int> Posts { get; set; }
    }
}
