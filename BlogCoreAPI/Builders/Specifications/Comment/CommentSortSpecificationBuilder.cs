using DBAccess.Data;
using DBAccess.Specifications.SortSpecification;

namespace BlogCoreAPI.Builders.Specifications.Comment
{
    /// <summary>
    /// Class used to generate <see cref="SortSpecification{TEntity}"/> for <see cref="Comment"/>.
    /// </summary>
    public class CommentSortSpecificationBuilder
    {
        private readonly string _sortingDirection;
        private readonly string _orderBy;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentSortSpecificationBuilder"/> class.
        /// </summary>
        /// <param name="sortingDirection"></param>
        /// <param name="orderBy"></param>
        public CommentSortSpecificationBuilder(string sortingDirection, string orderBy)
        {
            _sortingDirection = sortingDirection;
            _orderBy = orderBy;
        }

        /// <summary>
        /// Get sort specification of <see cref="Comment"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public SortSpecification<DBAccess.Data.Comment> Build()
        {
            SortSpecification<DBAccess.Data.Comment> sort;
            if (_orderBy == "LIKE")
                sort = new SortSpecification<DBAccess.Data.Comment>(new OrderBySpecification<DBAccess.Data.Comment>(x => x.Likes),
                    _sortingDirection == "DESC"
                        ? SortingDirectionSpecification.Descending
                        : SortingDirectionSpecification.Ascending);
            else
                sort = new SortSpecification<DBAccess.Data.Comment>(new OrderBySpecification<DBAccess.Data.Comment>(x => x.PublishedAt),
                    _sortingDirection == "DESC"
                        ? SortingDirectionSpecification.Descending
                        : SortingDirectionSpecification.Ascending);
            return sort;
        }
    }
}
