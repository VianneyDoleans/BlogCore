using DbAccess.Specifications.FilterSpecifications;
using DbAccess.Specifications.FilterSpecifications.Filters;

namespace MyBlogAPI.Filters.Comment
{
    public class CommentQueryFilter
    {
        private readonly string _authorUsername;
        private readonly string _postParentName;
        private readonly int? _minimumPostNumber;
        private readonly int? _maximumPostNumber;

        public CommentQueryFilter(string authorUsername, string postParentName, int? minimumPostNumber, int? maximumPostNumber)
        {
            _authorUsername = authorUsername;
            _postParentName = postParentName;
            _minimumPostNumber = minimumPostNumber;
            _maximumPostNumber = maximumPostNumber;
        }

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

            /*if (_maximumPostNumber != null)
            {
                filter = filter == null
                    ? new MaximumPostNumberSpecification<DbAccess.Data.POCO.Category>(_maximumPostNumber.Value)
                    : filter & new MaximumPostNumberSpecification<DbAccess.Data.POCO.Category>(_maximumPostNumber.Value);
            }*/

            return filter;
        }
    }
}
