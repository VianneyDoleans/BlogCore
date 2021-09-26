using DbAccess.Specifications.SortSpecification;

namespace MyBlogAPI.Filters.Category
{
    public class SortCategoryFilter
    {
        private readonly string _sortingDirection;

        public SortCategoryFilter(string sortingDirection)
        {
            _sortingDirection = sortingDirection;
        }

        public SortSpecification<DbAccess.Data.POCO.Category> GetSorting()
        {
            var sort = new SortSpecification<DbAccess.Data.POCO.Category>(new OrderBySpecification<DbAccess.Data.POCO.Category>(x => x.Name),
                _sortingDirection == "DESC"
                    ? SortingDirectionSpecification.Descending
                    : SortingDirectionSpecification.Ascending);
            return sort;
        }
    }
}
