using DbAccess.Specifications.SortSpecification;

namespace MyBlogAPI.Filters.Comment
{
    /// <summary>
    /// Class used to generate <see cref="SortSpecification{TEntity}"/> for <see cref="DbAccess.Data.POCO.Comment"/>.
    /// </summary>
    public class SortCommentFilter
    {
        private readonly string _sortingDirection;
        private readonly string _orderBy;

        /// <summary>
        /// Initializes a new instance of the <see cref="SortCommentFilter"/> class.
        /// </summary>
        /// <param name="sortingDirection"></param>
        /// <param name="orderBy"></param>
        public SortCommentFilter(string sortingDirection, string orderBy)
        {
            _sortingDirection = sortingDirection;
            _orderBy = orderBy;
        }

        /// <summary>
        /// Get sort specification of <see cref="DbAccess.Data.POCO.Comment"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public SortSpecification<DbAccess.Data.POCO.Comment> GetSorting()
        {
            SortSpecification<DbAccess.Data.POCO.Comment> sort;
            if (_orderBy == "LIKE")
                sort = new SortSpecification<DbAccess.Data.POCO.Comment>(new OrderBySpecification<DbAccess.Data.POCO.Comment>(x => x.Likes),
                    _sortingDirection == "DESC"
                        ? SortingDirectionSpecification.Descending
                        : SortingDirectionSpecification.Ascending);
            else
                sort = new SortSpecification<DbAccess.Data.POCO.Comment>(new OrderBySpecification<DbAccess.Data.POCO.Comment>(x => x.PublishedAt),
                    _sortingDirection == "DESC"
                        ? SortingDirectionSpecification.Descending
                        : SortingDirectionSpecification.Ascending);
            return sort;
        }
    }
}
