namespace BlogCoreAPI.Responses
{
    /// <summary>
    /// Error Response return from API to Client when the request couldn't be execute properly.
    /// </summary>
    public class BlogErrorResponse
    {
        /// <summary>
        /// Type of the exception raised.
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Message intended to the Client about the error description.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogErrorResponse"/> class.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        public BlogErrorResponse(string type, string message)
        {
            Type = type;
            Message = message;
        }
    }
}
