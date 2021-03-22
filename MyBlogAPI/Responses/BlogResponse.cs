using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlogAPI.Errors
{
    public class BlogResponse<T>
    {
        public BlogResponse(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
    }
}
