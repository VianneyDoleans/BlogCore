using DbAccess.Specifications.SortSpecification;

namespace MyBlogAPI.Filters.Role
{
    public class SortRoleFilter
    {
        private readonly string _sortingDirection;

        public SortRoleFilter(string sortingDirection)
        {
            _sortingDirection = sortingDirection;
        }

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
