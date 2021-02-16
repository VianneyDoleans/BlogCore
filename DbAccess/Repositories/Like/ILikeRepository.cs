using System.Collections.Generic;
using System.Threading.Tasks;

namespace DbAccess.Repositories.Like
{
    public interface ILikeRepository : IRepository<Data.POCO.Like>
    {
        Task<IEnumerable<Data.POCO.Like>> GetLikesFromPost(int id);

        Task<IEnumerable<Data.POCO.Like>> GetLikesFromUser(int id);

        Task<IEnumerable<Data.POCO.Like>> GetLikesFromComment(int id);
    }
}
