using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DbAccess.Repositories.Comment
{
    public interface ICommentRepository : IRepository<Data.POCO.Comment>
    { 
        Task<IQueryable<Data.POCO.Comment>> GetCommentsFromPost(int id);
        Task<IQueryable<Data.POCO.Comment>> GetCommentsFromUser(int id);
    }
}
