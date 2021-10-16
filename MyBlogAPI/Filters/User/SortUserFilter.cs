using DbAccess.Specifications.SortSpecification;

namespace MyBlogAPI.Filters.User
{
    /// <summary>
    /// Class used to generate <see cref="SortSpecification{TEntity}"/> for <see cref="DbAccess.Data.POCO.User"/>.
    /// </summary>
    public class SortUserFilter
    {
        private readonly string _sortingDirection;
        private readonly string _orderBy;

        /// <summary>
        /// Initializes a new instance of the <see cref="SortUserFilter"/> class.
        /// </summary>
        /// <param name="sortingDirection"></param>
        /// <param name="orderBy"></param>
        public SortUserFilter(string sortingDirection, string orderBy)
        {
            _sortingDirection = sortingDirection;
            _orderBy = orderBy;
        }

        /// <summary>
        /// Get sort specification of <see cref="DbAccess.Data.POCO.User"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
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
