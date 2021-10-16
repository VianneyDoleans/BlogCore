
namespace DbAccess.Specifications.SortSpecification
{
    public class Sort<TEntity>
    {
        public OrderBySpecification<TEntity> OrderBy;
        public SortingDirectionSpecification SortingDirection;
    }
}
