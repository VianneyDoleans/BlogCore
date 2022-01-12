﻿using System.Collections.Generic;

namespace MyBlogAPI.DTO.Post
{
    /// <summary>
    /// Interface of <see cref="DbAccess.Data.POCO.Post"/> Dto containing all the common properties of Post Dto Type (GET, ADD, UPDATE).
    /// </summary>
    public interface IPostDto
    {
        public string Content { get; set; }

        public string Name { get; set; }

        public int Author { get; set; }

        public int Category { get; set; }

        public IEnumerable<int> Tags { get; set; }
    }
}