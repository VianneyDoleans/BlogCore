using DBAccess.Data;
using DBAccess.Specifications.FilterSpecifications;
using DBAccess.Specifications.FilterSpecifications.Filters;

namespace BlogCoreAPI.Builders.Specifications.Comment
{
    /// <summary>
    /// Class used to generate <see cref="FilterSpecification{TEntity}"/> for <see cref="Comment"/>.
    /// </summary>
    public class CommentFilterSpecificationBuilder
    {
        private readonly string _authorUsername;
        private readonly string _postParentName;
        private readonly string _content;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentFilterSpecificationBuilder"/> class.
        /// </summary>
        /// <param name="authorUsername"></param>
        /// <param name="postParentName"></param>
        /// <param name="content"></param>
        public CommentFilterSpecificationBuilder(string authorUsername, string postParentName, string content)
        {
            _authorUsername = authorUsername;
            _postParentName = postParentName;
            _content = content;
        }

        /// <summary>
        /// Get filter specification of <see cref="Comment"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public FilterSpecification<DBAccess.Data.Comment> Build()
        {
            FilterSpecification<DBAccess.Data.Comment> filter = null;
            if (!string.IsNullOrEmpty(_authorUsername))
                filter = new AuthorUsernameContainsSpecification<DBAccess.Data.Comment>(_authorUsername);
            if (!string.IsNullOrEmpty(_postParentName))
            {
                filter = filter == null
                    ? new PostParentNameContains<DBAccess.Data.Comment>(_postParentName)
                    : filter & new PostParentNameContains<DBAccess.Data.Comment>(_postParentName);
            }
            if (!string.IsNullOrEmpty(_content))
            {
                filter = filter == null
                    ? new ContentContainsSpecification<DBAccess.Data.Comment>(_content)
                    : filter & new PostParentNameContains<DBAccess.Data.Comment>(_content);
            }

            return filter;
        }
    }
}
