using System.ComponentModel;
using BlogCoreAPI.Models.Queries.Interfaces;
using BlogCoreAPI.Models.Sort;
using Microsoft.AspNetCore.Mvc;

namespace BlogCoreAPI.Models.Queries
{
    public class GetCategoryQueryParameters : APaginationQuery, IOrderQuery
    {
        /// <summary>
        /// Order the return categories by Ascending or Descending
        /// </summary>
        [DefaultValue(Order.Asc)]
        [FromQuery(Name = "orderBy")]
        public Order OrderBy { get; set; } = Order.Asc;

        /// <summary>
        /// Sort the returned categories by the given parameter.
        /// </summary>
        [DefaultValue(CategorySort.Name)]
        [FromQuery(Name = "sortBy")]
        public CategorySort SortBy { get; set; } = CategorySort.Name;

        /// <summary>
        /// Returns only categories whose name contains the given parameter
        /// </summary>
        [FromQuery(Name = "inName")]
        public string InName { get; set; } = null;

        /// <summary>
        /// Returns only categories with a number of posts greater than or equal to the given parameter
        /// </summary>
        [FromQuery(Name = "minimumPosts")]
        public int? MinimumPosts { get; set; } = null;

        /// <summary>
        /// Returns only categories with a number of posts less than or equal to the given parameter
        /// </summary>
        [FromQuery(Name = "maximumPosts")]
        public int? MaximumPosts { get; set; } = null;
    }
}
