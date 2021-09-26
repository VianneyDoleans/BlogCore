using System;
using System.Linq.Expressions;
using DbAccess.Data.POCO.Interface;

namespace DbAccess.Specifications.FilterSpecifications.Filters
{
    public class RegisterBeforeDateSpecification<TEntity> : FilterSpecification<TEntity> where TEntity : class, IPoco, IHasRegisteredAt
    {
        private readonly DateTime _date;

        public RegisterBeforeDateSpecification(DateTime date)
        {
            _date = date;
        }

        protected override Expression<Func<TEntity, bool>> SpecificationExpression => p => p.RegisteredAt >= _date;
    }
}
