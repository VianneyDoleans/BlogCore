using System;
using System.Linq;
using System.Linq.Expressions;

namespace DbAccess.Specifications.FilterSpecifications
{
    public abstract class FilterSpecification<T>
    {
        private class ConstructedSpecification<TType> : FilterSpecification<TType>
        {
            public ConstructedSpecification(Expression<Func<TType, bool>> specificationExpression)
            {
                SpecificationExpression = specificationExpression;
            }

            protected override Expression<Func<TType, bool>> SpecificationExpression { get; }
        }

        protected abstract Expression<Func<T, bool>> SpecificationExpression { get; }

        public static implicit operator Expression<Func<T, bool>>(FilterSpecification<T> spec) => spec.SpecificationExpression;

        public static FilterSpecification<T> operator &(FilterSpecification<T> left, FilterSpecification<T> right) => CombineSpecification(left, right, Expression.AndAlso);

        public static FilterSpecification<T> operator |(FilterSpecification<T> left, FilterSpecification<T> right) => CombineSpecification(left, right, Expression.OrElse);

        private static FilterSpecification<T> CombineSpecification(FilterSpecification<T> left, FilterSpecification<T> right, Func<Expression, Expression, BinaryExpression> combiner)
        {
            var expr1 = left.SpecificationExpression;
            var expr2 = right.SpecificationExpression;
            var arg = Expression.Parameter(typeof(T));
            var combined = combiner.Invoke(
                new ReplaceParameterVisitor { { expr1.Parameters.Single(), arg } }.Visit(expr1.Body),
                new ReplaceParameterVisitor { { expr2.Parameters.Single(), arg } }.Visit(expr2.Body));
            return new ConstructedSpecification<T>(Expression.Lambda<Func<T, bool>>(combined, arg));
        }

        public FilterSpecification<T> And(FilterSpecification<T> other)
        {
            return this & other;
        }

        public FilterSpecification<T> Or(FilterSpecification<T> other)
        {
            return this | other;
        }

        public static FilterSpecification<T> operator !(FilterSpecification<T> original)
            => new ConstructedSpecification<T>(Expression.Lambda<Func<T, bool>>(Expression.Negate(original.SpecificationExpression.Body), original.SpecificationExpression.Parameters));

    }
}
