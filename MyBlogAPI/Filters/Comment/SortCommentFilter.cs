using DbAccess.Specifications.SortSpecification;

namespace MyBlogAPI.Filters.Comment
{
    public class SortCommentFilter
    {
        private readonly string _sortingDirection;
        private readonly string _orderBy;

        public SortCommentFilter(string sortingDirection, string orderBy)
        {
            _sortingDirection = sortingDirection;
            _orderBy = orderBy;
        }

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
