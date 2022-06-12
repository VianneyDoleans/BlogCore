using DbAccess.Specifications.FilterSpecifications;
using DbAccess.Specifications.FilterSpecifications.Filters;

namespace MyBlogAPI.Filters.Role
{
    /// <summary>
    /// Class used to generate <see cref="FilterSpecification{TEntity}"/> for <see cref="DbAccess.Data.POCO.Role"/>.
    /// </summary>
    public class RoleQueryFilter
    {
        private readonly string _name;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleQueryFilter"/> class.
        /// </summary>
        /// <param name="name"></param>
        public RoleQueryFilter(string name)
        {
            _name = name;
        }

        /// <summary>
        /// Get filter specification of <see cref="DbAccess.Data.POCO.Role"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public FilterSpecification<DbAccess.Data.POCO.Role> GetFilterSpecification()
        {
            FilterSpecification<DbAccess.Data.POCO.Role> filter = null;

            if (_name != null)
                filter = new NameContainsSpecification<DbAccess.Data.POCO.Role>(_name);

            return filter;
        }
    }
}
