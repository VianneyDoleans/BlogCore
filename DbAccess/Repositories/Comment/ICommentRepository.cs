using System.Collections.Generic;
using System.Threading.Tasks;

namespace DbAccess.Repositories.Comment
{
    public interface ICommentRepository : IRepository<Data.POCO.Comment>
    {
        /// <summary>
        /// Method used to see the existing comments from a post giving its Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<Data.POCO.Comment>> GetCommentsFromPost(int id);

        /// <summary>
        /// Method used to see the existing comments from a user giving its Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<Data.POCO.Comment>> GetCommentsFromUser(int id);
    }
}
