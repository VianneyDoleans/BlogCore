using System;
using System.Linq.Expressions;
using DBAccess.Contracts;
using DBAccess.Data;

namespace DBAccess.Specifications.FilterSpecifications.Filters
{
    public class RegisterAfterDateSpecification<TEntity> : FilterSpecification<TEntity> where TEntity : class, IPoco, IHasRegisteredAt
    {
        private readonly DateTime _date;

        public RegisterAfterDateSpecification(DateTime date)
        {
            _date = date;
        }

        protected override Expression<Func<TEntity, bool>> SpecificationExpression => p => p.RegisteredAt <= _date;
    }
}
