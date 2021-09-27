using DbAccess.Specifications.FilterSpecifications;
using DbAccess.Specifications.FilterSpecifications.Filters;

namespace MyBlogAPI.Filters.Comment
{
    /// <summary>
    /// Class used to generate <see cref="FilterSpecification{TEntity}"/> for <see cref="DbAccess.Data.POCO.Comment"/>.
    /// </summary>
    public class CommentQueryFilter
    {
        private readonly string _authorUsername;
        private readonly string _postParentName;
        private readonly string _content;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentQueryFilter"/> class.
        /// </summary>
        /// <param name="authorUsername"></param>
        /// <param name="postParentName"></param>
        /// <param name="content"></param>
        public CommentQueryFilter(string authorUsername, string postParentName, string content)
        {
            _authorUsername = authorUsername;
            _postParentName = postParentName;
            _content = content;
        }

        /// <summary>
        /// Get filter specification of <see cref="DbAccess.Data.POCO.Comment"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public FilterSpecification<DbAccess.Data.POCO.Comment> GetFilterSpecification()
        {
            FilterSpecification<DbAccess.Data.POCO.Comment> filter = null;
            if (!string.IsNullOrEmpty(_authorUsername))
                filter = new AuthorUsernameContainsSpecification<DbAccess.Data.POCO.Comment>(_authorUsername);
            if (!string.IsNullOrEmpty(_postParentName))
            {
                filter = filter == null
                    ? new PostParentNameContains<DbAccess.Data.POCO.Comment>(_postParentName)
                    : filter & new PostParentNameContains<DbAccess.Data.POCO.Comment>(_postParentName);
            }
            if (!string.IsNullOrEmpty(_content))
            {
                filter = filter == null
                    ? new ContentContainsSpecification<DbAccess.Data.POCO.Comment>(_content)
                    : filter & new PostParentNameContains<DbAccess.Data.POCO.Comment>(_content);
            }

            return filter;
        }
    }
}
