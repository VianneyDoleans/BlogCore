using System.Linq;
using System.Threading.Tasks;
using DBAccess.DataContext;
using Microsoft.EntityFrameworkCore;

namespace DBAccess.Repositories.Tag
{
    public class TagRepository : Repository<Data.POCO.Tag>, ITagRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TagRepository"/> class.
        /// </summary>
        /// <param name="context"></param>
        public TagRepository(MyBlogContext context) : base(context)
        {
        }

        /// <inheritdoc />
        public async Task<bool> NameAlreadyExists(string name)
        {
            var tag = await Context.Set<Data.POCO.Tag>().Where(x => x.Name == name).FirstOrDefaultAsync();
            return tag != null;
        }
    }
}
