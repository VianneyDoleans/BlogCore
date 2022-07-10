using System;
using System.ComponentModel;
using BlogCoreAPI.Models.Sort;
using Microsoft.AspNetCore.Mvc;

namespace BlogCoreAPI.Models.Queries
{
    public class GetUserQueryParameters
    {
        [DefaultValue(Order.Desc)]
        [FromQuery(Name ="order")]
        public Order Order { get; set; } = Order.Desc;

        [DefaultValue(UserSort.RegisteredAt)]
        [FromQuery(Name = "sort")]
        public UserSort Sort { get; set; } = UserSort.RegisteredAt;

        [DefaultValue(1)]
        [FromQuery(Name = "page")]
        public int Page { get; set; } = 1;

        [DefaultValue(10)]
        [FromQuery(Name = "pageSize")]
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Returns only users whose username contains the given parameter
        /// </summary>
        [FromQuery(Name = "inUserName")]
        public string InUserName { get; set; } = null;

        /// <summary>
        /// Returns only users whose register date is more recent or equal to the given parameter
        /// </summary>
        [FromQuery(Name = "FromRegisterDate")]
        public DateTime? FromRegisterDate { get; set; } = null;

        /// <summary>
        /// Returns only users whose register date is older or equal to the given parameter
        /// </summary>
        [FromQuery(Name = "ToRegisterDate")]
        public DateTime? ToRegisterDate { get; set; } = null;

        /// <summary>
        /// Returns only users whose last login date is more recent or equal to the given parameter
        /// </summary>
        [FromQuery(Name = "FromLastLoginDate")]
        public DateTime? FromLastLoginDate { get; set; } = null;

        /// <summary>
        /// Returns only users whose last login date is older or equal to the given parameter
        /// </summary>
        [FromQuery(Name = "ToLastLoginDate")]
        public DateTime? ToLastLoginDate { get; set; } = null;
    }
}
