using System.ComponentModel;
using BlogCoreAPI.Models.Queries.Interfaces;
using BlogCoreAPI.Models.Sort;
using Microsoft.AspNetCore.Mvc;

namespace BlogCoreAPI.Models.Queries
{
    public class GetCommentQueryParameters : APaginationQuery, IOrderQuery
    {
        /// <summary>
        /// Order the return comments by Ascending or Descending
        /// </summary>
        [DefaultValue(Order.Desc)]
        [FromQuery(Name = "orderBy")]
        public Order OrderBy { get; set; } = Order.Desc;

        /// <summary>
        /// Sort the returned comments by the given parameter.
        /// </summary>
        [DefaultValue(CommentSort.Publication)]
        [FromQuery(Name = "sortBy")]
        public CommentSort SortBy { get; set; } = CommentSort.Publication;

        /// <summary>
        /// Returns only comments whose author's username contains the given parameter
        /// </summary>
        [FromQuery(Name = "inAuthorUserName")]
        public string InAuthorUserName { get; set; } = null;

        /// <summary>
        /// Returns only comments whose parent post name contains the given parameter
        /// </summary>
        [FromQuery(Name = "inPostParentName")]
        public string InPostParentName { get; set; } = null;

        /// <summary>
        /// Returns only comments whose content contains the given parameter
        /// </summary>
        [FromQuery(Name = "inContent")]
        public string InContent { get; set; } = null;
    }
}
