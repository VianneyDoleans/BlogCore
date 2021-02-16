using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DbAccess.Repositories.Comment
{
    public interface ICommentsRepository : IRepository<Data.POCO.Comment>
    { 
        Task<IEnumerable<Data.POCO.Comment>> GetCommentsFromPost(int id);
        Task<IEnumerable<Data.POCO.Comment>> GetCommentsFromUser(int id);
    }
}
