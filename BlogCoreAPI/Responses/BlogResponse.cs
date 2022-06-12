using System.Collections.Generic;

namespace MyBlogAPI.Responses
{
    /// <summary>
    /// Response return from API to Client when the request was executed properly.
    /// </summary>
    public class BlogResponse<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlogResponse{T}"/> class.
        /// </summary>
        /// <param name="data"></param>
        public BlogResponse(IEnumerable<T> data)
        {
            Data = data;
        }

        /// <summary>
        /// Data returned to the Client after the execution of its request.
        /// </summary>
        public IEnumerable<T> Data { get; set; }
    }
}
