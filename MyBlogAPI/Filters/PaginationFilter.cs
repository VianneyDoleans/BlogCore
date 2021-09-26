namespace MyBlogAPI.Filters
{
    public class PaginationFilter
    {
        public int PageNumber { get; }
        public int PageSize { get; }


        public PaginationFilter(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize > 10 ? 10 : pageSize;
        }
    }
}
