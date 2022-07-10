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

        /// <summary>
        /// Returns only posts whose name contains the given parameter
        /// </summary>
        [FromQuery(Name = "inName")]
        public string InName { get; set; } = null;

        /// <summary>
        /// Returns only posts whose content contains the given parameter
        /// </summary>
        [FromQuery(Name = "inContent")]
        public string InContent { get; set; } = null;

        /// <summary>
        /// Returns only posts whose published date is more recent or equal to the given parameter
        /// </summary>
        [FromQuery(Name = "fromPublishedDate")]
        public DateTime? FromPublishedDate { get; set; } = null;

        /// <summary>
        /// Returns only posts whose published date is older or equal to the given parameter
        /// </summary>
        [FromQuery(Name = "toPublishedDate")]
        public DateTime? ToPublishedDate { get; set; } = null;

        /// <summary>
        /// Returns only posts with a number of likes greater than or equal to the given parameter
        /// </summary>
        [FromQuery(Name = "minimumLikeCount")]
        public int? MinimumLikeCount { get; set; } = null;

        /// <summary>
        /// Returns only posts with a number of likes greater than or equal to the given parameter
        /// </summary>
        [FromQuery(Name = "maximumLikeCount")]
        public int? MaximumLikeCount { get; set; } = null;

        /// <summary>
        /// Returns only posts that have the specified tag given in the parameter
        /// </summary>
        [FromQuery(Name = "tagged")]
        public List<string> Tagged { get; set; } = null;
    }
}
