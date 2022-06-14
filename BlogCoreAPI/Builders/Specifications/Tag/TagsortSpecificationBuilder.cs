using DBAccess.Data;
using DBAccess.Specifications.SortSpecification;

namespace BlogCoreAPI.Builders.Specifications.Tag
{
    /// <summary>
    /// Class used to generate <see cref="SortSpecification{TEntity}"/> for <see cref="Tag"/>.
    /// </summary>
    public class TagsortSpecificationBuilder
    {
        private readonly string _sortingDirection;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagsortSpecificationBuilder"/> class.
        /// </summary>
        /// <param name="sortingDirection"></param>
        public TagsortSpecificationBuilder(string sortingDirection)
        {
            _sortingDirection = sortingDirection;
        }

        /// <summary>
        /// Get sort specification of <see cref="Tag"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public SortSpecification<DBAccess.Data.Tag> Build()
        {
            var sort = new SortSpecification<DBAccess.Data.Tag>(
                new OrderBySpecification<DBAccess.Data.Tag>(x => x.Name), 
                _sortingDirection == "DESC" 
                    ? SortingDirectionSpecification.Descending 
                    : SortingDirectionSpecification.Ascending);
            return sort;
        }
    }
}
