using System.Collections.Generic;

namespace MyBlogAPI.Responses
{
    /// <summary>
    /// Class inherited from <see cref="BlogResponse{T}"/>.
    /// This class return a response to the request of the Client with pagination information.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedBlogResponse<T> : BlogResponse<T>
    {
        /// <summary>
        /// Offset of query elements applied.
        /// </summary>
        public int Offset { get; set; }

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
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="total"></param>
        public PagedBlogResponse(IList<T> data, int offset, int limit, int total) : base(data)
        {
            Offset = offset;
            Limit = limit;
            Data = data;
            Total = total;
        }
    }
}
