using DbAccess.Specifications.FilterSpecifications;
using DbAccess.Specifications.FilterSpecifications.Filters;

namespace MyBlogAPI.Filters.Role
{
    public class RoleQueryFilter
    {
        private readonly string _name;

        public RoleQueryFilter(string name)
        {
            _name = name;
        }

        public FilterSpecification<DbAccess.Data.POCO.Role> GetFilterSpecification()
        {
            FilterSpecification<DbAccess.Data.POCO.Role> filter = null;

            if (_name != null)
                filter = new NameContainsSpecification<DbAccess.Data.POCO.Role>(_name);

            return filter;
        }
    }
}
