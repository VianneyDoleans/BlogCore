using System.Collections.Generic;
using System.Threading.Tasks;

namespace DbAccess.Repositories.Comment
{
    public interface ICommentRepository : IRepository<Data.POCO.Comment>
    {
        Task<IEnumerable<Data.POCO.Comment>> GetCommentsFromPost(int id);
        Task<IEnumerable<Data.POCO.Comment>> GetCommentsFromUser(int id);
    }
}
