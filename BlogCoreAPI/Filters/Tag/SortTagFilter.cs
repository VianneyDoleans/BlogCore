using DBAccess.Data.POCO;
using DBAccess.Specifications.SortSpecification;

namespace BlogCoreAPI.Filters.Tag
{
    /// <summary>
    /// Class used to generate <see cref="SortSpecification{TEntity}"/> for <see cref="Tag"/>.
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
        /// Get sort specification of <see cref="Tag"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public SortSpecification<DBAccess.Data.POCO.Tag> GetSorting()
        {
            var sort = new SortSpecification<DBAccess.Data.POCO.Tag>(
                new OrderBySpecification<DBAccess.Data.POCO.Tag>(x => x.Name), 
                _sortingDirection == "DESC" 
                    ? SortingDirectionSpecification.Descending 
                    : SortingDirectionSpecification.Ascending);
            return sort;
        }
    }
}
