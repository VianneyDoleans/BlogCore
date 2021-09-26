using System.Collections.Generic;

namespace DbAccess.Specifications.SortSpecification
{
    public class SortSpecification<TEntity>
    {

        public List<Sort<TEntity>> SortElements { get; }

        public SortSpecification(OrderBySpecification<TEntity> orderBy, SortingDirectionSpecification sortingDirection)
        {
            SortElements = new List<Sort<TEntity>>()
            {
                new Sort<TEntity>() {OrderBy = orderBy, SortingDirection = sortingDirection}
            };
        }

        public static SortSpecification<TEntity> operator &(SortSpecification<TEntity> left, SortSpecification<TEntity> right) => CombineSpecification(left, right);

        private static SortSpecification<TEntity> CombineSpecification(SortSpecification<TEntity> left, SortSpecification<TEntity> right)
        {
            left.SortElements.AddRange(right.SortElements);
            return left;
        }

        public SortSpecification<TEntity> And(SortSpecification<TEntity> other)
        {
            return this & other;
        }
    }
}
