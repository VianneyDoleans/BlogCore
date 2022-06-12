using System.Collections.Generic;

namespace BlogCoreAPI.Responses
{
    /// <summary>
    /// Class inherited from <see cref="BlogResponse{T}"/>.
    /// This class return a response to the request of the Client with pagination information.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedBlogResponse<T> : BlogResponse<T>
    {
        /// <summary>
        /// Page of query elements applied.
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Limit of item numbers applied from the query requested.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Total number of items existing in the query requested.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedBlogResponse{T}"/> class.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="total"></param>
        public PagedBlogResponse(IEnumerable<T> data, int page, int size, int total) : base(data)
        {
            Page = page;
            Limit = size;
            Data = data;
            Total = total;
        }
    }
}
