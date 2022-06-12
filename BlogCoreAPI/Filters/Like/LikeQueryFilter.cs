using DBAccess.Data.POCO;
using DBAccess.Specifications.FilterSpecifications;
using DBAccess.Specifications.FilterSpecifications.Filters;

namespace BlogCoreAPI.Filters.Like
{
    /// <summary>
    /// Class used to generate <see cref="FilterSpecification{TEntity}"/> for <see cref="Like"/>.
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
        /// Get filter specification of <see cref="Like"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public FilterSpecification<DBAccess.Data.POCO.Like> GetFilterSpecification()
        {

            FilterSpecification<DBAccess.Data.POCO.Like> filter = null;

            if (_likeableType != null)
                filter = new LikeableTypeSpecification<DBAccess.Data.POCO.Like>(_likeableType.Value);

            return filter;
        }
    }
}
