using DBAccess.Data;
using DBAccess.Specifications.SortSpecification;

namespace BlogCoreAPI.Builders.Specifications.Role
{
    /// <summary>
    /// Class used to generate <see cref="SortSpecification{TEntity}"/> for <see cref="Role"/>.
    /// </summary>
    public class RoleSortSpecificationBuilder
    {
        private readonly string _sortingDirection;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleSortSpecificationBuilder"/> class.
        /// </summary>
        /// <param name="sortingDirection"></param>
        public RoleSortSpecificationBuilder(string sortingDirection)
        {
            _sortingDirection = sortingDirection;
        }

        /// <summary>
        /// Get sort specification of <see cref="Role"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public SortSpecification<DBAccess.Data.Role> Build()
        {
            var sort = new SortSpecification<DBAccess.Data.Role>(
                new OrderBySpecification<DBAccess.Data.Role>(x => x.Name),
                _sortingDirection == "DESC"
                    ? SortingDirectionSpecification.Descending
                    : SortingDirectionSpecification.Ascending);
            return sort;
        }
    }
}
