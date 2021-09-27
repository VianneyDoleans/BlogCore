using DbAccess.Specifications.FilterSpecifications;
using DbAccess.Specifications.FilterSpecifications.Filters;

namespace MyBlogAPI.Filters.Post
{
    /// <summary>
    /// Class used to generate <see cref="FilterSpecification{TEntity}"/> for <see cref="DbAccess.Data.POCO.Post"/>.
    /// </summary>
    public class PostQueryFilter
    {

        private readonly string _name;
        private readonly string _content;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostQueryFilter"/> class.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="name"></param>
        public PostQueryFilter(string content, string name)
        {
            _content = content;
            _name = name;
        }

        /// <summary>
        /// Get filter specification of <see cref="DbAccess.Data.POCO.Post"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
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
