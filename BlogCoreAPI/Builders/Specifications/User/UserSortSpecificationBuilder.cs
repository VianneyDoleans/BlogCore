using DBAccess.Data;
using DBAccess.Specifications.SortSpecification;

namespace BlogCoreAPI.Builders.Specifications.User
{
    /// <summary>
    /// Class used to generate <see cref="SortSpecification{TEntity}"/> for <see cref="User"/>.
    /// </summary>
    public class UserSortSpecificationBuilder
    {
        private readonly string _sortingDirection;
        private readonly string _orderBy;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSortSpecificationBuilder"/> class.
        /// </summary>
        /// <param name="sortingDirection"></param>
        /// <param name="orderBy"></param>
        public UserSortSpecificationBuilder(string sortingDirection, string orderBy)
        {
            _sortingDirection = sortingDirection;
            _orderBy = orderBy;
        }

        /// <summary>
        /// Get sort specification of <see cref="User"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public SortSpecification<DBAccess.Data.User> Build()
        {
            var sort = _orderBy switch
            {
                "USERNAME" => new SortSpecification<DBAccess.Data.User>(
                    new OrderBySpecification<DBAccess.Data.User>(x => x.UserName),
                    _sortingDirection == "DESC"
                        ? SortingDirectionSpecification.Descending
                        : SortingDirectionSpecification.Ascending),
                _ => new SortSpecification<DBAccess.Data.User>(
                    new OrderBySpecification<DBAccess.Data.User>(x => x.RegisteredAt),
                    _sortingDirection == "DESC"
                        ? SortingDirectionSpecification.Descending
                        : SortingDirectionSpecification.Ascending)
            };
            return sort;
        }
    }
}
