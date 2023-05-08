using System;
using System.ComponentModel;
using BlogCoreAPI.Models.Queries.Interfaces;
using BlogCoreAPI.Models.Sort;
using Microsoft.AspNetCore.Mvc;

namespace BlogCoreAPI.Models.Queries
{
    public class GetUserQueryParameters : APaginationQuery, IOrderQuery
    {
        /// <summary>
        /// Order the return users by Ascending or Descending
        /// </summary>
        [DefaultValue(Order.Desc)]
        [FromQuery(Name = "orderBy")]
        public Order OrderBy { get; set; } = Order.Desc;

        /// <summary>
        /// Sort the returned users by the given parameter.
        /// </summary>
        [DefaultValue(UserSort.Registration)]
        [FromQuery(Name = "sortBy")]
        public UserSort SortBy { get; set; } = UserSort.Registration;

        /// <summary>
        /// Returns only users whose username contains the given parameter
        /// </summary>
        [FromQuery(Name = "inUserName")]
        public string InUserName { get; set; } = null;

        /// <summary>
        /// Returns only users whose register date is more recent or equal to the given parameter
        /// </summary>
        [FromQuery(Name = "FromRegistrationDate")]
        public DateTimeOffset? FromRegistrationDate { get; set; } = null;

        /// <summary>
        /// Returns only users whose register date is older or equal to the given parameter
        /// </summary>
        [FromQuery(Name = "ToRegistrationDate")]
        public DateTimeOffset? ToRegistrationDate { get; set; } = null;

        /// <summary>
        /// Returns only users whose last login date is more recent or equal to the given parameter
        /// </summary>
        [FromQuery(Name = "FromLastLoginDate")]
        public DateTimeOffset? FromLastLoginDate { get; set; } = null;

        /// <summary>
        /// Returns only users whose last login date is older or equal to the given parameter
        /// </summary>
        [FromQuery(Name = "ToLastLoginDate")]
        public DateTimeOffset? ToLastLoginDate { get; set; } = null;
    }
}
