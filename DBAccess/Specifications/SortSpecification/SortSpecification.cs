using System.Collections.Generic;

namespace DBAccess.Specifications.SortSpecification
{
    public class SortSpecification<TEntity>
    {

        public List<Sort<TEntity>> SortElements { get; }

        public SortSpecification(OrderBySpecification<TEntity> sort, SortingDirectionSpecification order)
        {
            SortElements = new List<Sort<TEntity>>()
            {
                new() {OrderBy = sort, SortingDirection = order}
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
