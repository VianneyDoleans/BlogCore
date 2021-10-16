using DbAccess.Specifications.SortSpecification;

namespace MyBlogAPI.Filters.Category
{
    /// <summary>
    /// Class used to generate <see cref="SortSpecification{TEntity}"/> for <see cref="DbAccess.Data.POCO.Category"/>.
    /// </summary>
    public class SortCategoryFilter
    {
        private readonly string _sortingDirection;

        /// <summary>
        /// Initializes a new instance of the <see cref="SortCategoryFilter"/> class.
        /// </summary>
        /// <param name="sortingDirection"></param>
        public SortCategoryFilter(string sortingDirection)
        {
            _sortingDirection = sortingDirection;
        }

        /// <summary>
        /// Get sort specification of <see cref="DbAccess.Data.POCO.Category"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public SortSpecification<DbAccess.Data.POCO.Category> GetSorting()
        {
            var sort = new SortSpecification<DbAccess.Data.POCO.Category>(new OrderBySpecification<DbAccess.Data.POCO.Category>(x => x.Name),
                _sortingDirection == "DESC"
                    ? SortingDirectionSpecification.Descending
                    : SortingDirectionSpecification.Ascending);
            return sort;
        }
    }
}
