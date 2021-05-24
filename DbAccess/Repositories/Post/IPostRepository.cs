using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbAccess.Repositories.Post
{
    public interface IPostRepository : IRepository<Data.POCO.Post>
    {
        public Task<IQueryable<Data.POCO.Post>> GetPostsFromUser(int id);
        public Task<IQueryable<Data.POCO.Post>> GetPostsFromTag(int id);
        public Task<IQueryable<Data.POCO.Post>> GetPostsFromCategory(int id);

        public Task<bool> NameAlreadyExists(string name);
    }
}
