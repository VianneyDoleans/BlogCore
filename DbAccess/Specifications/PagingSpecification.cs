
namespace DbAccess.Specifications
{
    public class OrderBySpecification
    {
        public OrderBySpecification(int skip, int take)
        {
            Skip = skip;
            Take = take;
        }

        public int Skip { get; }
        public int Take { get; }
    }
}
