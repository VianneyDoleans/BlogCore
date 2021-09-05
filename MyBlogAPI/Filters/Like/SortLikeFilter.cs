using DbAccess.Specifications.SortSpecification;

namespace MyBlogAPI.Filters.Like
{
    public class SortLikeFilter
    {
        private readonly string _sortingDirection;

        public SortLikeFilter(string sortingDirection)
        {
            _sortingDirection = sortingDirection;
        }

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
