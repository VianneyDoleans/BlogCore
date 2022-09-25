using DBAccess.Data;
using DBAccess.Specifications.FilterSpecifications;
using DBAccess.Specifications.FilterSpecifications.Filters;

namespace BlogCoreAPI.Builders.Specifications.Like
{
    /// <summary>
    /// Class used to generate <see cref="FilterSpecification{TEntity}"/> for <see cref="Like"/>.
    /// </summary>
    public class LikeFilterSpecificationBuilder
    {
        private readonly LikeableType? _likeableType;

        /// <summary>
        /// Initializes a new instance of the <see cref="LikeFilterSpecificationBuilder"/> class.
        /// </summary>
        /// <param name="likeableType"></param>
        public LikeFilterSpecificationBuilder(LikeableType? likeableType)
        {
            _likeableType = likeableType;
        }

        /// <summary>
        /// Get filter specification of <see cref="Like"/> based of internal properties defined.
        /// </summary>
        /// <returns></returns>
        public FilterSpecification<DBAccess.Data.Like> Build()
        {

            FilterSpecification<DBAccess.Data.Like> filter = null;

            if (_likeableType != null)
                filter = new LikeableTypeSpecification<DBAccess.Data.Like>(_likeableType.Value);

            return filter;
        }
    }
}
