using System;
using System.Linq.Expressions;
using DBAccess.Data.POCO.Interface;

namespace DBAccess.Specifications.FilterSpecifications.Filters
{
    public class LastLoginBeforeDateSpecification<TEntity> : FilterSpecification<TEntity> where TEntity : class, IPoco, IHasLastLogin
    {
        private readonly DateTime _date;

        public LastLoginBeforeDateSpecification(DateTime date)
        {
            _date = date;
        }

        protected override Expression<Func<TEntity, bool>> SpecificationExpression => p => p.LastLogin >= _date;
    }
}
