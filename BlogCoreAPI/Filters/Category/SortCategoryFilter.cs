using DBAccess.Data.POCO;
using DBAccess.Specifications.SortSpecification;

namespace BlogCoreAPI.Filters.Category
{
    /// <summary>
    /// Class used to generate <see cref="SortSpecification{TEntity}"/> for <see cref="Category"/>.
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
        /// Get sort specification of <see cref="Category"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public SortSpecification<DBAccess.Data.POCO.Category> GetSorting()
        {
            var sort = new SortSpecification<DBAccess.Data.POCO.Category>(new OrderBySpecification<DBAccess.Data.POCO.Category>(x => x.Name),
                _sortingDirection == "DESC"
                    ? SortingDirectionSpecification.Descending
                    : SortingDirectionSpecification.Ascending);
            return sort;
        }
    }
}
