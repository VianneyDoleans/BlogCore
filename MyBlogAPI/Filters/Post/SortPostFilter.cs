using DbAccess.Specifications.SortSpecification;

namespace MyBlogAPI.Filters.Post
{
    public class SortPostFilter
    {
        private readonly string _sortingDirection;
        private readonly string _orderBy;

        public SortPostFilter(string sortingDirection, string orderBy)
        {
            _sortingDirection = sortingDirection;
            _orderBy = orderBy;
        }

        public SortSpecification<DbAccess.Data.POCO.Post> GetSorting()
        {
            var sort = _orderBy switch
            {
                "LIKE" => new SortSpecification<DbAccess.Data.POCO.Post>(
                    new OrderBySpecification<DbAccess.Data.POCO.Post>(x => x.Likes),
                    _sortingDirection == "DESC"
                        ? SortingDirectionSpecification.Descending
                        : SortingDirectionSpecification.Ascending),
                "NAME" => new SortSpecification<DbAccess.Data.POCO.Post>(
                    new OrderBySpecification<DbAccess.Data.POCO.Post>(x => x.Name),
                    _sortingDirection == "DESC"
                        ? SortingDirectionSpecification.Descending
                        : SortingDirectionSpecification.Ascending),
                _ => new SortSpecification<DbAccess.Data.POCO.Post>(
                    new OrderBySpecification<DbAccess.Data.POCO.Post>(x => x.PublishedAt),
                    _sortingDirection == "DESC"
                        ? SortingDirectionSpecification.Descending
                        : SortingDirectionSpecification.Ascending)
            };
            return sort;
        }
    }
}
