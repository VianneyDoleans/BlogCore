using DbAccess.Specifications.FilterSpecifications;
using DbAccess.Specifications.FilterSpecifications.Filters;

namespace MyBlogAPI.Filters.Category
{
    public class CategoryQueryFilter
    {
        private readonly string _name;
        private readonly int? _minimumPostNumber;
        private readonly int? _maximumPostNumber;

        public CategoryQueryFilter(string name, int? minimumPostNumber, int? maximumPostNumber)
        {
            _name = name;
            _minimumPostNumber = minimumPostNumber;
            _maximumPostNumber = maximumPostNumber;
        }

        public FilterSpecification<DbAccess.Data.POCO.Category> GetFilterSpecification()
        {
            FilterSpecification<DbAccess.Data.POCO.Category> filter = null;
            if (!string.IsNullOrEmpty(_name))
                filter = new NameContainsSpecification<DbAccess.Data.POCO.Category>(_name);
            if (_minimumPostNumber != null)
            {
                filter = filter == null
                    ? new MinimumPostNumberSpecification<DbAccess.Data.POCO.Category>(_minimumPostNumber.Value)
                    : filter & new MinimumPostNumberSpecification<DbAccess.Data.POCO.Category>(_minimumPostNumber.Value);
            }

            if (_maximumPostNumber != null)
            {
                filter = filter == null
                    ? new MaximumPostNumberSpecification<DbAccess.Data.POCO.Category>(_maximumPostNumber.Value)
                    : filter & new MaximumPostNumberSpecification<DbAccess.Data.POCO.Category>(_maximumPostNumber.Value);
            }

            return filter;
        }
    }
}
