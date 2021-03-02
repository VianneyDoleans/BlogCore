using System.Threading.Tasks;

namespace DbAccess.Repositories.Category
{
    public interface ICategoryRepository : IRepository<Data.POCO.Category>
    {
        Task<bool> NameAlreadyExists(string name);
    }
}
