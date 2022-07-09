using DBAccess.Specifications.FilterSpecifications;
using DBAccess.Specifications.FilterSpecifications.Filters;

namespace BlogCoreAPI.Builders.Specifications.Tag
{
    /// <summary>
    /// Class used to generate <see cref="FilterSpecification{TEntity}"/> for <see cref="Tag"/>.
    /// </summary>
    public class TagQueryFilter
    {
        private readonly string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagQueryFilter"/> class.
        /// </summary>
        /// <param name="name"></param>
        public TagQueryFilter(string name)
        {
            _name = name;
        }

        /// <summary>
        /// Get filter specification of <see cref="Tag"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public FilterSpecification<DBAccess.Data.Tag> Build()
        {
            FilterSpecification<DBAccess.Data.Tag> filter = null;

            if (_name != null)
                filter = new NameContainsSpecification<DBAccess.Data.Tag>(_name);

            return filter;
        }
    }
}
