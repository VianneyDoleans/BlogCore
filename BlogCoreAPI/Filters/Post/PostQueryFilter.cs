using DBAccess.Data.POCO;
using DBAccess.Specifications.FilterSpecifications;
using DBAccess.Specifications.FilterSpecifications.Filters;

namespace BlogCoreAPI.Filters.Post
{
    /// <summary>
    /// Class used to generate <see cref="FilterSpecification{TEntity}"/> for <see cref="Post"/>.
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
        /// Get filter specification of <see cref="Post"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public FilterSpecification<DBAccess.Data.POCO.Post> GetFilterSpecification()
        {

            FilterSpecification<DBAccess.Data.POCO.Post> filter = null;

            if (_content != null)
                filter = new ContentContainsSpecification<DBAccess.Data.POCO.Post>(_content);
            if (_name != null)
            {
                filter = filter == null ?
                    new NameContainsSpecification<DBAccess.Data.POCO.Post>(_name)
                    : filter & new NameContainsSpecification<DBAccess.Data.POCO.Post>(_name);
            }

            return filter;
        }
    }
}
