using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBAccess.Repositories.Comment
{
    public interface ICommentRepository : IRepository<Data.Comment>
    {
        /// <summary>
        /// Method used to see the existing comments from a post giving its Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<Data.Comment>> GetCommentsFromPost(int id);

        /// <summary>
        /// Method used to see the existing comments from a user giving its Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<Data.Comment>> GetCommentsFromUser(int id);
    }
}
