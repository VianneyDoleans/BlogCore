using System;
using System.Collections.Generic;
using System.ComponentModel;
using BlogCoreAPI.Models.Queries.Interfaces;
using BlogCoreAPI.Models.Sort;
using Microsoft.AspNetCore.Mvc;

namespace BlogCoreAPI.Models.Queries
{
    public class GetPostQueryParameters : APaginationQuery, IOrderQuery
    {
        /// <summary>
        /// Order the return posts by Ascending or Descending
        /// </summary>
        [DefaultValue(Order.Desc)]
        [FromQuery(Name = "orderBy")]
        public Order OrderBy { get; set; } = Order.Desc;

        /// <summary>
        /// Sort the returned posts by the given parameter.
        /// </summary>
        [DefaultValue(PostSort.Publication)]
        [FromQuery(Name = "sortBy")]
        public PostSort SortBy { get; set; } = PostSort.Publication;

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
        /// Returns only posts whose publication date is more recent or equal to the given parameter
        /// </summary>
        [FromQuery(Name = "fromPublicationDate")]
        public DateTimeOffset? FromPublicationDate { get; set; } = null;

        /// <summary>
        /// Returns only posts whose published date is older or equal to the given parameter
        /// </summary>
        [FromQuery(Name = "toPublicationDate")]
        public DateTimeOffset? ToPublicationDate { get; set; } = null;

        /// <summary>
        /// Returns only posts with a number of likes greater than or equal to the given parameter
        /// </summary>
        [FromQuery(Name = "minimumLikes")]
        public int? MinimumLikes { get; set; } = null;

        /// <summary>
        /// Returns only posts with a number of likes greater than or equal to the given parameter
        /// </summary>
        [FromQuery(Name = "maximumLikes")]
        public int? MaximumLikes { get; set; } = null;

        /// <summary>
        /// Returns only posts that have the specified tag given in the parameter
        /// </summary>
        [FromQuery(Name = "tagged")]
        public List<string> Tagged { get; set; } = null;
    }
}
