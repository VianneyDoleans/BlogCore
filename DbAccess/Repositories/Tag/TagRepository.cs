using System.Linq;
using System.Threading.Tasks;
using DbAccess.DataContext;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Repositories.Tag
{
    public class TagRepository : Repository<Data.POCO.Tag>, ITagRepository
    {
        public TagRepository(MyBlogContext context) : base(context)
        {
        }
        public async Task<bool> NameAlreadyExists(string name)
        {
            var tag = await Context.Set<Data.POCO.Tag>().Where(x => x.Name == name).FirstOrDefaultAsync();
            return tag != null;
        }
    }
}
