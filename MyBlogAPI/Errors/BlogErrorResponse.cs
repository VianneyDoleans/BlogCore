using System;

namespace MyBlogAPI.Errors
{
    public class BlogErrorResponse
    {
        public string Type { get; set; }
        public string Message { get; set; }

        public BlogErrorResponse(Exception ex)
        {
            Type = ex.GetType().Name;
            Message = ex.Message;
        }
    }
}
