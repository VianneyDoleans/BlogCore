using System;

namespace MyBlogAPI.Responses
{
    /// <summary>
    /// Error Response return from API to Client when the request couldn't be execute properly.
    /// </summary>
    public class BlogErrorResponse
    {
        /// <summary>
        /// Type of the exception raised.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Message intended to the Client about the error description.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogErrorResponse"/> class.
        /// </summary>
        /// <param name="ex"></param>
        public BlogErrorResponse(Exception ex)
        {
            Type = ex.GetType().Name;
            Message = ex.Message;
        }
    }
}
