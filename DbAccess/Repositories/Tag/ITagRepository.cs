using System.Threading.Tasks;

namespace DbAccess.Repositories.Tag
{
    public interface ITagRepository : IRepository<Data.POCO.Tag>
    {
        Task<bool> NameAlreadyExists(string name);
    }
}
