using DbAccess.Specifications.FilterSpecifications;
using DbAccess.Specifications.FilterSpecifications.Filters;

namespace MyBlogAPI.Filters.Post
{
    public class PostQueryFilter
    {

        private readonly string _name;
        private readonly string _content;

        public PostQueryFilter(string content, string name)
        {
            _content = content;
            _name = name;
        }

        public FilterSpecification<DbAccess.Data.POCO.Post> GetFilterSpecification()
        {

            FilterSpecification<DbAccess.Data.POCO.Post> filter = null;

            if (_content != null)
                filter = new ContentContainsSpecification<DbAccess.Data.POCO.Post>(_content);
            if (_name != null)
            {
                if (filter == null)
                    filter = new NameContainsSpecification<DbAccess.Data.POCO.Post>(_name);
                else
                    filter &= new NameContainsSpecification<DbAccess.Data.POCO.Post>(_name);
            }

            return filter;
        }
    }
}
