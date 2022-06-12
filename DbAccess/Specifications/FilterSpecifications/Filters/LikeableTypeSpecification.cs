using System;
using System.Linq.Expressions;
using DBAccess.Data.POCO;
using DBAccess.Data.POCO.Interface;

namespace DBAccess.Specifications.FilterSpecifications.Filters
{
    public class LikeableTypeSpecification<TEntity> : FilterSpecification<TEntity> where TEntity : class, IPoco, IHasComment, IHasPost
    {
        private readonly LikeableType _type;

        public LikeableTypeSpecification(LikeableType type)
        {
            _type = type;
        }

        protected override Expression<Func<TEntity, bool>> SpecificationExpression
            => p => _type == LikeableType.Comment ? p.Comment != null : p.Post != null;
    }
}
