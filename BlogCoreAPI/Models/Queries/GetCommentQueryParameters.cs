using System.ComponentModel;
using BlogCoreAPI.Models.Sort;
using Microsoft.AspNetCore.Mvc;

namespace BlogCoreAPI.Models.Queries
{
    public class GetCommentQueryParameters
    {
        [DefaultValue(Order.Desc)]
        [FromQuery(Name = "order")]
        public Order Order { get; set; } = Order.Desc;

        [DefaultValue(CommentSort.PublishedAt)]

        [FromQuery(Name = "sort")]
        public CommentSort Sort { get; set; } = CommentSort.PublishedAt;

        [DefaultValue(1)]
        [FromQuery(Name = "page")]
        public int Page { get; set; } = 1;

        [DefaultValue(10)]
        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; } = 10;

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
