using System.ComponentModel;
using BlogCoreAPI.Models.Queries.Interfaces;
using DBAccess.Data;
using Microsoft.AspNetCore.Mvc;

namespace BlogCoreAPI.Models.Queries
{
    public class GetLikeQueryParameters : APaginationQuery, IOrderQuery
    {
        /// <summary>
        /// Order the return likes by Ascending or Descending
        /// </summary>
        [DefaultValue(Order.Desc)]
        [FromQuery(Name = "orderBy")]
        public Order OrderBy { get; set; } = Order.Desc;

        /// <summary>
        /// Returns only the likes that are on posts **or** comments according to the given parameter
        /// </summary>
        [FromQuery(Name = "likeableType")]
        public LikeableType? LikeableType { get; set; } = null;
    }
}
