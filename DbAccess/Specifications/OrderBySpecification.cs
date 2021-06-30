using System;
using System.Linq;
using System.Linq.Expressions;

namespace DbAccess.Specifications
{
    public class OrderBySpecification<T, TProperty>
    {

        public OrderBySpecification(Expression<Func<T, TProperty>> order)
        {
            Order = order;
        }

        public Expression<Func<T, TProperty>> Order { get; }
    }
}
