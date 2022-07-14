namespace BlogCoreAPI.Models.Queries.Interfaces
{
    public interface IPaginationQuery
    {
        public int Page { get; set; }

        public int PageSize { get; set; }
    }
}
