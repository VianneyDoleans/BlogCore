using DBAccess.Data.POCO;
using DBAccess.Specifications.SortSpecification;

namespace BlogCoreAPI.Filters.Comment
{
    /// <summary>
    /// Class used to generate <see cref="SortSpecification{TEntity}"/> for <see cref="Comment"/>.
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
        /// Get sort specification of <see cref="Comment"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public SortSpecification<DBAccess.Data.POCO.Comment> GetSorting()
        {
            SortSpecification<DBAccess.Data.POCO.Comment> sort;
            if (_orderBy == "LIKE")
                sort = new SortSpecification<DBAccess.Data.POCO.Comment>(new OrderBySpecification<DBAccess.Data.POCO.Comment>(x => x.Likes),
                    _sortingDirection == "DESC"
                        ? SortingDirectionSpecification.Descending
                        : SortingDirectionSpecification.Ascending);
            else
                sort = new SortSpecification<DBAccess.Data.POCO.Comment>(new OrderBySpecification<DBAccess.Data.POCO.Comment>(x => x.PublishedAt),
                    _sortingDirection == "DESC"
                        ? SortingDirectionSpecification.Descending
                        : SortingDirectionSpecification.Ascending);
            return sort;
        }
    }
}
