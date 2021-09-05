using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbAccess.Data.POCO;
using DbAccess.Specifications.FilterSpecifications;
using DbAccess.Specifications.FilterSpecifications.Filters;

namespace MyBlogAPI.Filters.Like
{
    public class LikeQueryFilter
    {
        private readonly LikeableType? _likeableType;

        public LikeQueryFilter(LikeableType? likeableType)
        {
            _likeableType = likeableType;
        }

        public FilterSpecification<DbAccess.Data.POCO.Like> GetFilterSpecification()
        {

            FilterSpecification<DbAccess.Data.POCO.Like> filter = null;

            if (_likeableType != null)
                filter = new LikeableTypeSpecification<DbAccess.Data.POCO.Like>(_likeableType.Value);

            return filter;
        }
    }
}
