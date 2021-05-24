using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbAccess.Repositories.Like
{
    public interface ILikeRepository : IRepository<Data.POCO.Like>
    {
        Task<IQueryable<Data.POCO.Like>> GetLikesFromPost(int id);

        Task<IQueryable<Data.POCO.Like>> GetLikesFromUser(int id);

        Task<IQueryable<Data.POCO.Like>> GetLikesFromComment(int id);

        Task<bool> LikeAlreadyExists(Data.POCO.Like like);
    }
}
