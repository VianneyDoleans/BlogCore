using DBAccess.Specifications.SortSpecification;

namespace BlogCoreAPI.Models.Builders.Specifications.Tag
{
    /// <summary>
    /// Class used to generate <see cref="SortSpecification{TEntity}"/> for <see cref="Tag"/>.
    /// </summary>
    public class TagSortSpecificationBuilder
    {
        private readonly Order _order;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagSortSpecificationBuilder"/> class.
        /// </summary>
        /// <param name="order"></param>
        public TagSortSpecificationBuilder(Order order)
        {
            _order = order;
        }

        /// <summary>
        /// Get sort specification of <see cref="Tag"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public SortSpecification<DBAccess.Data.Tag> Build()
        {
            var sort = new SortSpecification<DBAccess.Data.Tag>(
                new OrderBySpecification<DBAccess.Data.Tag>(x => x.Name), 
                _order == Order.Desc 
                    ? SortingDirectionSpecification.Descending 
                    : SortingDirectionSpecification.Ascending);
            return sort;
        }
    }
}
