using DbAccess.Specifications.FilterSpecifications;
using DbAccess.Specifications.FilterSpecifications.Filters;

namespace MyBlogAPI.Filters.Category
{
    /// <summary>
    /// Class used to generate <see cref="FilterSpecification{TEntity}"/> for <see cref="DbAccess.Data.POCO.Category"/>.
    /// </summary>
    public class CategoryQueryFilter
    {
        private readonly string _name;
        private readonly int? _minimumPostNumber;
        private readonly int? _maximumPostNumber;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryQueryFilter"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="minimumPostNumber"></param>
        /// <param name="maximumPostNumber"></param>
        public CategoryQueryFilter(string name, int? minimumPostNumber, int? maximumPostNumber)
        {
            _name = name;
            _minimumPostNumber = minimumPostNumber;
            _maximumPostNumber = maximumPostNumber;
        }

        /// <summary>
        /// Get filter specification of <see cref="DbAccess.Data.POCO.Category"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
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
