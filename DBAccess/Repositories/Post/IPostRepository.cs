using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBAccess.Repositories.Post
{
    public interface IPostRepository : IRepository<Data.Post>
    {
        /// <summary>
        /// Method used to see the existing posts from a user giving its Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<IEnumerable<Data.Post>> GetPostsFromUser(int id);

        /// <summary>
        /// Method used to see the existing posts from a tag giving its Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<IEnumerable<Data.Post>> GetPostsFromTag(int id);

        /// <summary>
        /// Method used to see the existing posts from a category giving its Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<IEnumerable<Data.Post>> GetPostsFromCategory(int id);

        /// <summary>
        /// Method used to check if a name already exists inside database for a post.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<bool> NameAlreadyExists(string name);
    }
}
