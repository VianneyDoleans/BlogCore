using DbAccess.Specifications.SortSpecification;

namespace MyBlogAPI.Filters.Tag
{
    /// <summary>
    /// Class used to generate <see cref="SortSpecification{TEntity}"/> for <see cref="DbAccess.Data.POCO.Tag"/>.
    /// </summary>
    public class SortTagFilter
    {
        private readonly string _sortingDirection;

        /// <summary>
        /// Initializes a new instance of the <see cref="SortTagFilter"/> class.
        /// </summary>
        /// <param name="sortingDirection"></param>
        public SortTagFilter(string sortingDirection)
        {
            _sortingDirection = sortingDirection;
        }

        /// <summary>
        /// Get sort specification of <see cref="DbAccess.Data.POCO.Tag"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public SortSpecification<DbAccess.Data.POCO.Tag> GetSorting()
        {
            var sort = new SortSpecification<DbAccess.Data.POCO.Tag>(
                new OrderBySpecification<DbAccess.Data.POCO.Tag>(x => x.Name), 
                _sortingDirection == "DESC" 
                    ? SortingDirectionSpecification.Descending 
                    : SortingDirectionSpecification.Ascending);
            return sort;
        }
    }
}
