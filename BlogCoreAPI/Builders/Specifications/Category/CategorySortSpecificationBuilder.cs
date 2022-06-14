using DBAccess.Data;
using DBAccess.Specifications.SortSpecification;

namespace BlogCoreAPI.Builders.Specifications.Category
{
    /// <summary>
    /// Class used to generate <see cref="SortSpecification{TEntity}"/> for <see cref="Category"/>.
    /// </summary>
    public class CategorySortSpecificationBuilder
    {
        private readonly string _sortingDirection;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategorySortSpecificationBuilder"/> class.
        /// </summary>
        /// <param name="sortingDirection"></param>
        public CategorySortSpecificationBuilder(string sortingDirection)
        {
            _sortingDirection = sortingDirection;
        }

        /// <summary>
        /// Get sort specification of <see cref="Category"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public SortSpecification<DBAccess.Data.Category> Build()
        {
            var sort = new SortSpecification<DBAccess.Data.Category>(new OrderBySpecification<DBAccess.Data.Category>(x => x.Name),
                _sortingDirection == "DESC"
                    ? SortingDirectionSpecification.Descending
                    : SortingDirectionSpecification.Ascending);
            return sort;
        }
    }
}
