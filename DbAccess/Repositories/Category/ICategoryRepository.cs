using System.Threading.Tasks;

namespace DBAccess.Repositories.Category
{
    /// <summary>
    /// Repository used to manipulate <see cref="Data.POCO.Category"/> from database (CRUD and more).
    /// </summary>
    public interface ICategoryRepository : IRepository<Data.POCO.Category>
    {
        /// <summary>
        /// Method used to check if a name already exists inside database for a category.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<bool> NameAlreadyExists(string name);
    }
}
