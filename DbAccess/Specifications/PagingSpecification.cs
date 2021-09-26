
namespace DbAccess.Specifications
{
    public class PagingSpecification
    {
        public PagingSpecification(int skip, int take)
        {
            Skip = skip;
            Take = take;
        }

        public int Skip { get; }
        public int Take { get; }
    }
}
