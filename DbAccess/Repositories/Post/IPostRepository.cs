using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbAccess.Specifications;

namespace DbAccess.Repositories.Post
{
    public interface IPostRepository : IRepository<Data.POCO.Post>
    {
        public Task<IEnumerable<Data.POCO.Post>> GetPostsFromUser(int id, PagingSpecification pagingSpecification = null);
        public Task<IEnumerable<Data.POCO.Post>> GetPostsFromTag(int id, PagingSpecification pagingSpecification = null);
        public Task<IEnumerable<Data.POCO.Post>> GetPostsFromCategory(int id, PagingSpecification pagingSpecification = null);

        public Task<bool> NameAlreadyExists(string name);
    }
}
