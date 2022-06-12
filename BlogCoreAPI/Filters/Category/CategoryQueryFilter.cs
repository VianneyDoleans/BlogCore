using DBAccess.Data.POCO;
using DBAccess.Specifications.FilterSpecifications;
using DBAccess.Specifications.FilterSpecifications.Filters;

namespace BlogCoreAPI.Filters.Category
{
    /// <summary>
    /// Class used to generate <see cref="FilterSpecification{TEntity}"/> for <see cref="Category"/>.
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
        /// Get filter specification of <see cref="Category"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public FilterSpecification<DBAccess.Data.POCO.Category> GetFilterSpecification()
        {
            FilterSpecification<DBAccess.Data.POCO.Category> filter = null;
            if (!string.IsNullOrEmpty(_name))
                filter = new NameContainsSpecification<DBAccess.Data.POCO.Category>(_name);
            if (_minimumPostNumber != null)
            {
                filter = filter == null
                    ? new MinimumPostNumberSpecification<DBAccess.Data.POCO.Category>(_minimumPostNumber.Value)
                    : filter & new MinimumPostNumberSpecification<DBAccess.Data.POCO.Category>(_minimumPostNumber.Value);
            }

            if (_maximumPostNumber != null)
            {
                filter = filter == null
                    ? new MaximumPostNumberSpecification<DBAccess.Data.POCO.Category>(_maximumPostNumber.Value)
                    : filter & new MaximumPostNumberSpecification<DBAccess.Data.POCO.Category>(_maximumPostNumber.Value);
            }

            return filter;
        }
    }
}
