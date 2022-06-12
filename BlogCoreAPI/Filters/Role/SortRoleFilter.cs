using DBAccess.Data.POCO;
using DBAccess.Specifications.SortSpecification;

namespace BlogCoreAPI.Filters.Role
{
    /// <summary>
    /// Class used to generate <see cref="SortSpecification{TEntity}"/> for <see cref="Role"/>.
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
        /// Get sort specification of <see cref="Role"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public SortSpecification<DBAccess.Data.POCO.Role> GetSorting()
        {
            var sort = new SortSpecification<DBAccess.Data.POCO.Role>(
                new OrderBySpecification<DBAccess.Data.POCO.Role>(x => x.Name),
                _sortingDirection == "DESC"
                    ? SortingDirectionSpecification.Descending
                    : SortingDirectionSpecification.Ascending);
            return sort;
        }
    }
}
