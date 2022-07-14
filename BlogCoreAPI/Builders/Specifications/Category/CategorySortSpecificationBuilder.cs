using BlogCoreAPI.Models;
using BlogCoreAPI.Models.Sort;
using DBAccess.Specifications.SortSpecification;

namespace BlogCoreAPI.Builders.Specifications.Category
{
    /// <summary>
    /// Class used to generate <see cref="SortSpecification{TEntity}"/> for <see cref="Category"/>.
    /// </summary>
    public class CategorySortSpecificationBuilder
    {
        private readonly Order _order;
        private readonly CategorySort _sort;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategorySortSpecificationBuilder"/> class.
        /// </summary>
        /// <param name="order"></param>
        /// <param name="categorySort"></param>
        public CategorySortSpecificationBuilder(Order order, CategorySort categorySort)
        {
            _order = order;
            _sort = categorySort;
        }

        /// <summary>
        /// Get sort specification of <see cref="Category"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public SortSpecification<DBAccess.Data.Category> Build()
        {

            var sort = _sort switch
            {
                CategorySort.Posts => new SortSpecification<DBAccess.Data.Category>(
                    new OrderBySpecification<DBAccess.Data.Category>(x => x.Posts.Count),
                    _order == Order.Desc
                        ? SortingDirectionSpecification.Descending
                        : SortingDirectionSpecification.Ascending),
                CategorySort.Name => new SortSpecification<DBAccess.Data.Category>(
                    new OrderBySpecification<DBAccess.Data.Category>(x => x.Name),
                    _order == Order.Desc
                        ? SortingDirectionSpecification.Descending
                        : SortingDirectionSpecification.Ascending),
                _ => new SortSpecification<DBAccess.Data.Category>(
                    new OrderBySpecification<DBAccess.Data.Category>(x => x.Name),
                    _order == Order.Desc
                        ? SortingDirectionSpecification.Descending
                        : SortingDirectionSpecification.Ascending)
            };
            
            return sort;
        }
    }
}
