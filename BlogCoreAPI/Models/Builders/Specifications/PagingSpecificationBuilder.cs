using DBAccess.Specifications;

namespace BlogCoreAPI.Builders.Specifications
{
    /// <summary>
    /// Pagination filter used to execute Query on a resource.
    /// </summary>
    public class PagingSpecificationBuilder
    {
        /// <summary>
        /// Page of the query.
        /// </summary>
        public int Page { get; }

        /// <summary>
        /// Limit of the query.
        /// </summary>
        public int Limit { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagingSpecificationBuilder"/> class.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        public PagingSpecificationBuilder(int page, int pageSize)
        {
            Page = page < 1 ? 1 : page;
            Limit = pageSize > 100 ? 100 : pageSize;
        }

        public PagingSpecification Build()
        {
            return new PagingSpecification((Page - 1) * Limit, Limit);
        }
    }
}
