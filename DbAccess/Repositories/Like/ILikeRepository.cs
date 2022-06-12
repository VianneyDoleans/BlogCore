using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBAccess.Repositories.Like
{
    public interface ILikeRepository : IRepository<Data.POCO.Like>
    {
        /// <summary>
        /// Method used to see the existing likes from a post giving its Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<Data.POCO.Like>> GetLikesFromPost(int id);

        /// <summary>
        /// Method used to see the existing likes from a user giving its Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<Data.POCO.Like>> GetLikesFromUser(int id);

        /// <summary>
        /// Method used to see the existing likes from a comment giving its Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<Data.POCO.Like>> GetLikesFromComment(int id);

        /// <summary>
        /// Method used to check if a like already exists inside database.
        /// </summary>
        /// <param name="like"></param>
        /// <returns></returns>
        Task<bool> LikeAlreadyExists(Data.POCO.Like like);
    }
}
