using DBAccess.Data.POCO;
using DBAccess.Specifications.SortSpecification;

namespace BlogCoreAPI.Filters.User
{
    /// <summary>
    /// Class used to generate <see cref="SortSpecification{TEntity}"/> for <see cref="User"/>.
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
        /// Get sort specification of <see cref="User"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public SortSpecification<DBAccess.Data.POCO.User> GetSorting()
        {
            var sort = _orderBy switch
            {
                "USERNAME" => new SortSpecification<DBAccess.Data.POCO.User>(
                    new OrderBySpecification<DBAccess.Data.POCO.User>(x => x.UserName),
                    _sortingDirection == "DESC"
                        ? SortingDirectionSpecification.Descending
                        : SortingDirectionSpecification.Ascending),
                _ => new SortSpecification<DBAccess.Data.POCO.User>(
                    new OrderBySpecification<DBAccess.Data.POCO.User>(x => x.RegisteredAt),
                    _sortingDirection == "DESC"
                        ? SortingDirectionSpecification.Descending
                        : SortingDirectionSpecification.Ascending)
            };
            return sort;
        }
    }
}
