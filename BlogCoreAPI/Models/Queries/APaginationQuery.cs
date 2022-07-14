using System.ComponentModel;
using BlogCoreAPI.Models.Queries.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlogCoreAPI.Models.Queries
{
    public abstract class APaginationQuery : IPaginationQuery
    {
        /// <summary>
        /// Cursor for pagination across multiple pages of results. Default value is 1.
        /// </summary>
        [DefaultValue(1)]
        [FromQuery(Name = "page")]
        public int Page { get; set; } = 1;

        /// <summary>
        /// Number of elements to return per page. Max value is 100. Default value is 10.
        /// </summary>
        [DefaultValue(10)]
        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; } = 10;
    }
}