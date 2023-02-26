using DBAccess.Specifications.FilterSpecifications;
using DBAccess.Specifications.FilterSpecifications.Filters;

namespace BlogCoreAPI.Models.Builders.Specifications.Tag
{
    /// <summary>
    /// Class used to generate <see cref="FilterSpecification{TEntity}"/> for <see cref="Tag"/>.
    /// </summary>
    public class TagQueryFilter
    {
        private readonly string _inName;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagQueryFilter"/> class.
        /// </summary>
        /// <param name="inName"></param>
        public TagQueryFilter(string inName)
        {
            _inName = inName;
        }

        /// <summary>
        /// Get filter specification of <see cref="Tag"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public FilterSpecification<DBAccess.Data.Tag> Build()
        {
            FilterSpecification<DBAccess.Data.Tag> filter = null;

            if (_inName != null)
                filter = new NameContainsSpecification<DBAccess.Data.Tag>(_inName);

            return filter;
        }
    }
}
