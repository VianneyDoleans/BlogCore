using DbAccess.Specifications.SortSpecification;

namespace MyBlogAPI.Filters.User
{
    public class SortUserFilter
    {
        private readonly string _sortingDirection;
        private readonly string _orderBy;

        public SortUserFilter(string sortingDirection, string orderBy)
        {
            _sortingDirection = sortingDirection;
            _orderBy = orderBy;
        }

        public SortSpecification<DbAccess.Data.POCO.User> GetSorting()
        {
            var sort = _orderBy switch
            {
                "USERNAME" => new SortSpecification<DbAccess.Data.POCO.User>(
                    new OrderBySpecification<DbAccess.Data.POCO.User>(x => x.Username),
                    _sortingDirection == "DESC"
                        ? SortingDirectionSpecification.Descending
                        : SortingDirectionSpecification.Ascending),
                _ => new SortSpecification<DbAccess.Data.POCO.User>(
                    new OrderBySpecification<DbAccess.Data.POCO.User>(x => x.RegisteredAt),
                    _sortingDirection == "DESC"
                        ? SortingDirectionSpecification.Descending
                        : SortingDirectionSpecification.Ascending)
            };
            return sort;
        }
    }
}
