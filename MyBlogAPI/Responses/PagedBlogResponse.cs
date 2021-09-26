using System.Collections.Generic;

namespace MyBlogAPI.Responses
{
    public class PagedBlogResponse<T> : BlogResponse<T>
    {
        public int Offset { get; set; }
        public int Limit { get; set; }
        public int Total { get; set; }

        public PagedBlogResponse(IList<T> data, int offset, int limit, int total) : base(data)
        {
            Offset = offset;
            Limit = limit;
            Data = data;
            Total = total;
        }
    }
}
