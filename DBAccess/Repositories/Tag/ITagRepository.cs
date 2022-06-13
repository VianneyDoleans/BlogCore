using System.Threading.Tasks;

namespace DBAccess.Repositories.Tag
{
    public interface ITagRepository : IRepository<Data.Tag>
    {
        /// <summary>
        /// Method used to check if a name already exists inside database for a tag.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<bool> NameAlreadyExists(string name);
    }
}
