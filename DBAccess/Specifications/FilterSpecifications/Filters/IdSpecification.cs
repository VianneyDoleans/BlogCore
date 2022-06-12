using System;
using System.Linq.Expressions;
using DBAccess.Data.POCO.Interface;

namespace DBAccess.Specifications.FilterSpecifications.Filters
{
    public class IdSpecification<TEntity> : FilterSpecification<TEntity> where TEntity : class, IPoco
    {
        private readonly int _id;

        public IdSpecification(int id)
        {
            _id = id;
        }

        protected override Expression<Func<TEntity, bool>> SpecificationExpression => p => p.Id == _id;
    }
}
