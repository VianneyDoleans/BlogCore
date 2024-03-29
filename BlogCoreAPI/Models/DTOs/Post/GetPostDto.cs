﻿#nullable enable
using System;
using System.Collections.Generic;

namespace BlogCoreAPI.Models.DTOs.Post
{
    /// <summary>
    /// GET Dto type of <see cref="Post"/>.
    /// </summary>
    public class GetPostDto : ADto, IPostDto
    {
        public string Content { get; set; }

        public string? ThumbnailUrl { get; set; }

        public string Name { get; set; }

        public int Author { get; set; }

        public int Category { get; set; }

        public DateTimeOffset PublishedAt { get; set; }

        public DateTimeOffset? ModifiedAt { get; set; }

        public IEnumerable<int> Likes { get; set; }

        public virtual IEnumerable<int> Tags { get; set; }
    }
}
