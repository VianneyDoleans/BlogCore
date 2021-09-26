namespace MyBlogAPI.Models
{
    public class PaginationParameters
    {
        public int Page { get; set; }
        public int Limit { get; set; }

        public PaginationParameters()
        {
            Page = 1;
            Limit = 10;
        }
    }
}
