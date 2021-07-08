using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbAccess.Specifications;

namespace DbAccess.Repositories.Comment
{
    public interface ICommentRepository : IRepository<Data.POCO.Comment>
    {
        Task<IEnumerable<Data.POCO.Comment>> GetCommentsFromPost(int id);
        Task<IEnumerable<Data.POCO.Comment>> GetCommentsFromUser(int id);
    }
}
