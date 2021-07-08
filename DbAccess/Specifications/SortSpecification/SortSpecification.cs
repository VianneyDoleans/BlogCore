
using System;
using System.Collections.Generic;

namespace DbAccess.Specifications.SortSpecification
{
    public class SortSpecification<TEntity>
    {

        public List<Tuple<OrderBySpecification<TEntity>, SortingDirectionSpecification>> SortElements { get; }

        public SortSpecification(OrderBySpecification<TEntity> orderBy, SortingDirectionSpecification sortingDirection)
        {
            SortElements = new List<Tuple<OrderBySpecification<TEntity>, SortingDirectionSpecification>>()
            {
                new Tuple<OrderBySpecification<TEntity>, SortingDirectionSpecification>(orderBy, sortingDirection)
            };
        }

        private void AddSort()
        {

        }

        public static SortSpecification<TEntity> operator &(SortSpecification<TEntity> left, SortSpecification<TEntity> right) => CombineSpecification(left, right);

        private static SortSpecification<TEntity> CombineSpecification(SortSpecification<TEntity> left, SortSpecification<TEntity> right)
        {
            foreach (var item in right.SortElements)
            {
                left.
            }
        }

        public SortSpecification<TEntity> And(SortSpecification<TEntity> other)
        {
            return this & other;
        }
    }
}
