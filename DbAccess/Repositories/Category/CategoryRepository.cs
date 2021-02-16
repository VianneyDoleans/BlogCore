using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbAccess.DataContext;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Repositories.Category
{
    public class CategoryRepository : Repository<Data.POCO.Category>, ICategoryRepository
    {
        public CategoryRepository(MyBlogContext context) : base(context)
        {
        }

        public override async Task<Data.POCO.Category> GetAsync(int id)
        {
            return await Context.Set<Data.POCO.Category>()
                .Include(x => x.Posts)
                .SingleAsync(x => x.Id == id);
        }

        public override Data.POCO.Category Get(int id)
        {
            return Context.Set<Data.POCO.Category>()
                .Include(x => x.Posts)
                .Single(x => x.Id == id);
        }

        public override IEnumerable<Data.POCO.Category> GetAll()
        {
            return Context.Set<Data.POCO.Category>()
                .Include(x => x.Posts)
                .ToList();
        }

        public override async Task<IEnumerable<Data.POCO.Category>> GetAllAsync()
        {
            return await Context.Set<Data.POCO.Category>()
                .Include(x => x.Posts)
                .ToListAsync();
        }
    }
}
