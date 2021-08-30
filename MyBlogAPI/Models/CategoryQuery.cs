using DbAccess.Data.POCO;
using DbAccess.Specifications;
using DbAccess.Specifications.FilterSpecifications;
using DbAccess.Specifications.SortSpecification;

namespace MyBlogAPI.Models
{
    public class CategoryQuery
    {
        public string Name { get; set; }
        public string MinimumPostNumber { get; set; }
        public string MaximumPostNumber { get; set; }
    }
}
