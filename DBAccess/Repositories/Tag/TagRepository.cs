using System.Linq;
using System.Threading.Tasks;
using DBAccess.DataContext;
using Microsoft.EntityFrameworkCore;

namespace DBAccess.Repositories.Tag
{
    public class TagRepository : Repository<Data.Tag>, ITagRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TagRepository"/> class.
        /// </summary>
        /// <param name="context"></param>
        public TagRepository(BlogCoreContext context) : base(context)
        {
        }

        /// <inheritdoc />
        public async Task<bool> NameAlreadyExists(string name)
        {
            var tag = await _context.Set<Data.Tag>().Where(x => x.Name == name).FirstOrDefaultAsync();
            return tag != null;
        }
    }
}
