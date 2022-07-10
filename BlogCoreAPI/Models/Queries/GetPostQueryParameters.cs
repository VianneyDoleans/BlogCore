using System;
using System.Collections.Generic;
using System.ComponentModel;
using BlogCoreAPI.Models.Sort;
using Microsoft.AspNetCore.Mvc;

namespace BlogCoreAPI.Models.Queries
{
    public class GetPostQueryParameters
    {
        [DefaultValue(Order.Desc)]
        [FromQuery(Name = "order")]
        public Order Order { get; set; } = Order.Desc;

        [DefaultValue(PostSort.PublishedAt)]
        [FromQuery(Name = "sort")]
        public PostSort Sort { get; set; } = PostSort.PublishedAt;

        [DefaultValue(1)]
        [FromQuery(Name = "page")]
        public int Page { get; set; } = 1;

        [DefaultValue(10)]
        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; } = 10;

        [FromQuery(Name = "inName")]
        public string InName { get; set; } = null;

        [FromQuery(Name = "inContent")]
        public string InContent { get; set; } = null;

        [FromQuery(Name = "fromPublishedDate")]
        public DateTime? FromPublishedDate { get; set; } = null;

        [FromQuery(Name = "toPublishedDate")]
        public DateTime? ToPublishedDate { get; set; } = null;

        [FromQuery(Name = "minimumLikeCount")]
        public int? MinimumLikeCount { get; set; } = null;

        [FromQuery(Name = "maximumLikeCount")]
        public int? MaximumLikeCount { get; set; } = null;

        [FromQuery(Name = "tagged")]
        public List<string> Tagged { get; set; } = null;
    }
}
