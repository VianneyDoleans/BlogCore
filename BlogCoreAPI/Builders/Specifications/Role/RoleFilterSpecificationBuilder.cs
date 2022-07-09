using DBAccess.Specifications.FilterSpecifications;
using DBAccess.Specifications.FilterSpecifications.Filters;

namespace BlogCoreAPI.Builders.Specifications.Role
{
    /// <summary>
    /// Class used to generate <see cref="FilterSpecification{TEntity}"/> for <see cref="Role"/>.
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
        /// Get filter specification of <see cref="Role"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public FilterSpecification<DBAccess.Data.Role> GetFilterSpecification()
        {
            FilterSpecification<DBAccess.Data.Role> filter = null;

            if (_name != null)
                filter = new NameContainsSpecification<DBAccess.Data.Role>(_name);

            return filter;
        }
    }
}
