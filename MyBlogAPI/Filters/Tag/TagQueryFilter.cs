using DbAccess.Specifications.FilterSpecifications;
using DbAccess.Specifications.FilterSpecifications.Filters;

namespace MyBlogAPI.Filters.Tag
{
    public class TagQueryFilter
    {
        private readonly string _name;

        public TagQueryFilter(string name)
        {
            _name = name;
        }

        public FilterSpecification<DbAccess.Data.POCO.Tag> GetFilterSpecification()
        {
            FilterSpecification<DbAccess.Data.POCO.Tag> filter = null;

            if (_name != null)
                filter = new NameContainsSpecification<DbAccess.Data.POCO.Tag>(_name);

            return filter;
        }
    }
}
