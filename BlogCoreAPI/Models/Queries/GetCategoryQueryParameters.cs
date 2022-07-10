using System.ComponentModel;
using BlogCoreAPI.Models.Sort;
using Microsoft.AspNetCore.Mvc;

namespace BlogCoreAPI.Models.Queries
{
    public class GetCategoryQueryParameters
    {
        [DefaultValue(Order.Asc)]
        [FromQuery(Name = "order")]
        public Order Order { get; set; } = Order.Asc;

        [DefaultValue(CategorySort.Name)]
        [FromQuery(Name = "sort")]
        public CategorySort Sort { get; set; } = CategorySort.Name;

        [DefaultValue(1)]
        [FromQuery(Name = "page")]
        public int Page { get; set; } = 1;

        [DefaultValue(10)]
        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Returns only categories whose name contains the given parameter
        /// </summary>
        [FromQuery(Name = "inName")]
        public string InName { get; set; } = null;

        /// <summary>
        /// Returns only categories with a number of posts greater than or equal to the given parameter
        /// </summary>
        [FromQuery(Name = "minimumPostCount")]
        public int? MinimumPostCount { get; set; } = null;

        /// <summary>
        /// Returns only categories with a number of posts less than or equal to the given parameter
        /// </summary>
        [FromQuery(Name = "maximumPostCount")]
        public int? MaximumPostCount { get; set; } = null;
    }
}
