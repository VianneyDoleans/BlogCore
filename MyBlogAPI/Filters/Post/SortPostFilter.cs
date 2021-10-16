using DbAccess.Specifications.SortSpecification;

namespace MyBlogAPI.Filters.Post
{
    /// <summary>
    /// Class used to generate <see cref="SortSpecification{TEntity}"/> for <see cref="DbAccess.Data.POCO.Post"/>.
    /// </summary>
    public class SortPostFilter
    {
        private readonly string _sortingDirection;
        private readonly string _orderBy;

        /// <summary>
        /// Initializes a new instance of the <see cref="SortPostFilter"/> class.
        /// </summary>
        /// <param name="sortingDirection"></param>
        /// <param name="orderBy"></param>
        public SortPostFilter(string sortingDirection, string orderBy)
        {
            _sortingDirection = sortingDirection;
            _orderBy = orderBy;
        }

        /// <summary>
        /// Get sort specification of <see cref="DbAccess.Data.POCO.Post"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
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
