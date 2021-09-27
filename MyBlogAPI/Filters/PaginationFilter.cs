namespace MyBlogAPI.Filters
{
    /// <summary>
    /// Pagination filter used to execute Query on a resource.
    /// </summary>
    public class PaginationFilter
    {
        /// <summary>
        /// Offset of the query.
        /// </summary>
        public int Offset { get; }

        /// <summary>
        /// Limit of the query.
        /// </summary>
        public int Limit { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginationFilter"/> class.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        public PaginationFilter(int offset, int limit)
        {
            Offset = offset < 1 ? 1 : offset;
            Limit = limit > 10 ? 10 : limit;
        }
    }
}
