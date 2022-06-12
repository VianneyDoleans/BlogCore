using DbAccess.Specifications.FilterSpecifications;
using DbAccess.Specifications.FilterSpecifications.Filters;

namespace MyBlogAPI.Filters.Tag
{
    /// <summary>
    /// Class used to generate <see cref="FilterSpecification{TEntity}"/> for <see cref="DbAccess.Data.POCO.Tag"/>.
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
        /// Get filter specification of <see cref="DbAccess.Data.POCO.Tag"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public FilterSpecification<DbAccess.Data.POCO.Tag> GetFilterSpecification()
        {
            FilterSpecification<DbAccess.Data.POCO.Tag> filter = null;

            if (_name != null)
                filter = new NameContainsSpecification<DbAccess.Data.POCO.Tag>(_name);

            return filter;
        }
    }
}
