using System.Threading.Tasks;

namespace DbAccess.Repositories.Category
{
    /// <summary>
    /// Repository used to manipulate <see cref="Data.POCO.Category"/> from database (CRUD and more).
    /// </summary>
    public interface ICategoryRepository : IRepository<Data.POCO.Category>
    {
        /// <summary>
        /// Method used to check if a name already exists inside database for a <see cref="Data.POCO.Category"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<bool> NameAlreadyExists(string name);
    }
}
