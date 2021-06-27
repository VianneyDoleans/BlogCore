using System;
using System.Linq.Expressions;

namespace DbAccess.Specifications.FilterSpecifications
{
    class SpecificIdSpecification<TEntity> : FilterSpecification<TEntity>
    {
        protected override Expression<Func<TEntity, bool>> SpecificationExpression => p => p.UnitPrice < 30;
    }
}
