using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbAccess.DataContext;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Repositories.Comment
{
    public class CommentsRepository : Repository<Data.POCO.Comment>, ICommentsRepository
    {
        public CommentsRepository(MyBlogContext context) : base(context)
        {
        }

        public override async Task<Data.POCO.Comment> GetAsync(int id)
        {
            return await Context.Set<Data.POCO.Comment>()
                .Include(x => x.PostParent)
                .Include(x => x.Author).SingleAsync(x => x.Id == id);
        }

        public override Data.POCO.Comment Get(int id)
        {
            return Context.Set<Data.POCO.Comment>()
                .Include(x => x.PostParent)
                .Include(x => x.Author).Single(x => x.Id == id);
        }

        public override IEnumerable<Data.POCO.Comment> GetAll()
        {
            return Context.Set<Data.POCO.Comment>()
                .Include(x => x.PostParent)
                .Include(x => x.Author).ToList();
        }

        public override async Task<IEnumerable<Data.POCO.Comment>> GetAllAsync()
        {
            return await Context.Set<Data.POCO.Comment>()
                .Include(x => x.PostParent)
                .Include(x => x.Author).ToListAsync();
        }

        public async Task<IEnumerable<Data.POCO.Comment>> GetCommentsFromPost(int id)
        {
            return await Context.Set<Data.POCO.Comment>()
                .Include(x => x.Author)
                .Include(x => x.PostParent).Where(x => x.PostParent.Id == id).ToListAsync();
        }

        public async Task<IEnumerable<Data.POCO.Comment>> GetCommentsFromUser(int id)
        {
            return await Context.Set<Data.POCO.Comment>()
                .Include(x => x.Author)
                .Include(x => x.PostParent).Where(x => x.Author.Id == id).ToListAsync();
        }
    }
}
