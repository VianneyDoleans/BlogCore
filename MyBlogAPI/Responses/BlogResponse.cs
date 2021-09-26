using System.Collections.Generic;

namespace MyBlogAPI.Responses
{
    public class BlogResponse<T>
    {
        public BlogResponse(IEnumerable<T> data)
        {
            Data = data;
        }

        public IEnumerable<T> Data { get; set; }
    }
}
