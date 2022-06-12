using DbAccess.Specifications.SortSpecification;

namespace MyBlogAPI.Filters.Like
{
    /// <summary>
    ///  Class used to generate <see cref="SortSpecification{TEntity}"/> for <see cref="DbAccess.Data.POCO.Like"/>.
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
        /// Get sort specification of <see cref="DbAccess.Data.POCO.Like"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public SortSpecification<DbAccess.Data.POCO.Like> GetSorting()
        {
            var sort = new SortSpecification<DbAccess.Data.POCO.Like>(
                new OrderBySpecification<DbAccess.Data.POCO.Like>(x => x.PublishedAt),
                _sortingDirection == "DESC"
                    ? SortingDirectionSpecification.Descending
                    : SortingDirectionSpecification.Ascending);
            return sort;
        }
    }
}
