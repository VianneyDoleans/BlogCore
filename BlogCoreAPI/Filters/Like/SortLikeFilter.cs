using DBAccess.Data.POCO;
using DBAccess.Specifications.SortSpecification;

namespace BlogCoreAPI.Filters.Like
{
    /// <summary>
    ///  Class used to generate <see cref="SortSpecification{TEntity}"/> for <see cref="Like"/>.
    /// </summary>
    public class SortLikeFilter
    {
        private readonly string _sortingDirection;

        /// <summary>
        /// Initializes a new instance of the <see cref="SortLikeFilter"/> class.
        /// </summary>
        /// <param name="sortingDirection"></param>
        public SortLikeFilter(string sortingDirection)
        {
            _sortingDirection = sortingDirection;
        }

        /// <summary>
        /// Get sort specification of <see cref="Like"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public SortSpecification<DBAccess.Data.POCO.Like> GetSorting()
        {
            var sort = new SortSpecification<DBAccess.Data.POCO.Like>(
                new OrderBySpecification<DBAccess.Data.POCO.Like>(x => x.PublishedAt),
                _sortingDirection == "DESC"
                    ? SortingDirectionSpecification.Descending
                    : SortingDirectionSpecification.Ascending);
            return sort;
        }
    }
}
