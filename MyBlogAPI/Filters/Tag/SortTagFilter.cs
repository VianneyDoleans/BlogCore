using DbAccess.Specifications.SortSpecification;

namespace MyBlogAPI.Filters.Tag
{
    public class SortTagFilter
    {
        private readonly string _sortingDirection;

        public SortTagFilter(string sortingDirection)
        {
            _sortingDirection = sortingDirection;
        }

        public SortSpecification<DbAccess.Data.POCO.Tag> GetSorting()
        {
            var sort = new SortSpecification<DbAccess.Data.POCO.Tag>(
                new OrderBySpecification<DbAccess.Data.POCO.Tag>(x => x.Name), 
                _sortingDirection == "DESC" 
                    ? SortingDirectionSpecification.Descending 
                    : SortingDirectionSpecification.Ascending);
            return sort;
        }
    }
}
