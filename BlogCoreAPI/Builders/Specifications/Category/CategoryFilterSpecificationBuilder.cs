using DBAccess.Data;
using DBAccess.Specifications.FilterSpecifications;
using DBAccess.Specifications.FilterSpecifications.Filters;

namespace BlogCoreAPI.Builders.Specifications.Category
{
    /// <summary>
    /// Class used to generate <see cref="FilterSpecification{TEntity}"/> for <see cref="Category"/>.
    /// </summary>
    public class CategoryFilterSpecificationBuilder
    {
        private readonly string _name;
        private readonly int? _minimumPostNumber;
        private readonly int? _maximumPostNumber;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryFilterSpecificationBuilder"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="minimumPostNumber"></param>
        /// <param name="maximumPostNumber"></param>
        public CategoryFilterSpecificationBuilder(string name, int? minimumPostNumber, int? maximumPostNumber)
        {
            _name = name;
            _minimumPostNumber = minimumPostNumber;
            _maximumPostNumber = maximumPostNumber;
        }

        /// <summary>
        /// Get filter specification of <see cref="Category"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public FilterSpecification<DBAccess.Data.Category> Build()
        {
            FilterSpecification<DBAccess.Data.Category> filter = null;
            if (!string.IsNullOrEmpty(_name))
                filter = new NameContainsSpecification<DBAccess.Data.Category>(_name);
            if (_minimumPostNumber != null)
            {
                filter = filter == null
                    ? new MinimumPostNumberSpecification<DBAccess.Data.Category>(_minimumPostNumber.Value)
                    : filter & new MinimumPostNumberSpecification<DBAccess.Data.Category>(_minimumPostNumber.Value);
            }

            if (_maximumPostNumber != null)
            {
                filter = filter == null
                    ? new MaximumPostNumberSpecification<DBAccess.Data.Category>(_maximumPostNumber.Value)
                    : filter & new MaximumPostNumberSpecification<DBAccess.Data.Category>(_maximumPostNumber.Value);
            }

            return filter;
        }
    }
}
