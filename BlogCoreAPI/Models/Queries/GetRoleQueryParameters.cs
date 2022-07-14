using System.ComponentModel;
using BlogCoreAPI.Models.Queries.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlogCoreAPI.Models.Queries
{
    public class GetRoleQueryParameters : APaginationQuery, IOrderQuery
    {
        /// <summary>
        /// Order the return roles by Ascending or Descending
        /// </summary>
        [DefaultValue(Order.Asc)]
        [FromQuery(Name = "orderBy")]
        public Order OrderBy { get; set; } = Order.Asc;
    }
}
