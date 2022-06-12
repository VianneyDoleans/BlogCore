
namespace DBAccess.Specifications.SortSpecification
{
    public class Sort<TEntity>
    {
        public OrderBySpecification<TEntity> OrderBy { get; set; }
        public SortingDirectionSpecification SortingDirection { get; set; }
    }
}
