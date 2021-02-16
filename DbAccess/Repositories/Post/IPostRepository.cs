using System.Collections.Generic;
using System.Threading.Tasks;

namespace DbAccess.Repositories.Post
{
    public interface IPostRepository : IRepository<Data.POCO.Post>
    {
        public Task<IEnumerable<Data.POCO.Post>> GetPostsFromUser(int id);
        public Task<IEnumerable<Data.POCO.Post>> GetPostsFromTag(int id);
        public Task<IEnumerable<Data.POCO.Post>> GetPostsFromCategory(int id);
    }
}
