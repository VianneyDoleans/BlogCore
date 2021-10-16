using DbAccess.Data.POCO;
using DbAccess.Specifications.FilterSpecifications;
using DbAccess.Specifications.FilterSpecifications.Filters;

namespace MyBlogAPI.Filters.Like
{
    /// <summary>
    /// Class used to generate <see cref="FilterSpecification{TEntity}"/> for <see cref="DbAccess.Data.POCO.Like"/>.
    /// </summary>
    public class LikeQueryFilter
    {
        private readonly LikeableType? _likeableType;

        /// <summary>
        /// Initializes a new instance of the <see cref="LikeQueryFilter"/> class.
        /// </summary>
        /// <param name="likeableType"></param>
        public LikeQueryFilter(LikeableType? likeableType)
        {
            _likeableType = likeableType;
        }

        /// <summary>
        /// Get filter specification of <see cref="DbAccess.Data.POCO.Like"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public FilterSpecification<DbAccess.Data.POCO.Like> GetFilterSpecification()
        {

            FilterSpecification<DbAccess.Data.POCO.Like> filter = null;

            if (_likeableType != null)
                filter = new LikeableTypeSpecification<DbAccess.Data.POCO.Like>(_likeableType.Value);

            return filter;
        }
    }
}
