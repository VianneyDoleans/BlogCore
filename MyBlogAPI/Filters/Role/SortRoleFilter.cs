using DbAccess.Specifications.SortSpecification;

namespace MyBlogAPI.Filters.Role
{
    /// <summary>
    /// Class used to generate <see cref="SortSpecification{TEntity}"/> for <see cref="DbAccess.Data.POCO.Role"/>.
    /// </summary>
    public class SortRoleFilter
    {
        private readonly string _sortingDirection;

        /// <summary>
        /// Initializes a new instance of the <see cref="SortRoleFilter"/> class.
        /// </summary>
        /// <param name="sortingDirection"></param>
        public SortRoleFilter(string sortingDirection)
        {
            _sortingDirection = sortingDirection;
        }

        /// <summary>
        /// Get sort specification of <see cref="DbAccess.Data.POCO.Role"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public SortSpecification<DbAccess.Data.POCO.Role> GetSorting()
        {
            var sort = new SortSpecification<DbAccess.Data.POCO.Role>(
                new OrderBySpecification<DbAccess.Data.POCO.Role>(x => x.Name),
                _sortingDirection == "DESC"
                    ? SortingDirectionSpecification.Descending
                    : SortingDirectionSpecification.Ascending);
            return sort;
        }
    }
}
