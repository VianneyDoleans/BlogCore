using System;
using System.Linq.Expressions;

namespace DbAccess.Specifications.SortSpecification
{
    public class OrderBySpecification<T>
    {

        public OrderBySpecification(Expression<Func<T, object>> order)
        {
            Order = order;
        }

        public Expression<Func<T, object>> Order { get; }
    }
}
