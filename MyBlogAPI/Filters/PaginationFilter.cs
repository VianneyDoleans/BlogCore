﻿namespace MyBlogAPI.Filters
{
    /// <summary>
    /// Pagination filter used to execute Query on a resource.
    /// </summary>
    public class PaginationFilter
    {
        /// <summary>
        /// Page of the query.
        /// </summary>
        public int Page { get; }

        /// <summary>
        /// Limit of the query.
        /// </summary>
        public int Limit { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginationFilter"/> class.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        public PaginationFilter(int page, int size)
        {
            Page = page < 1 ? 1 : page;
            Limit = size > 10 ? 10 : size;
        }
    }
}
