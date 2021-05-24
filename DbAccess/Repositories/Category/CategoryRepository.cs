using System;
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
            try
            {
                return await Context.Set<Data.POCO.Category>()
                    .Include(x => x.Posts)
                    .SingleAsync(x => x.Id == id);
            }
            catch
            {
                throw new IndexOutOfRangeException("Category doesn't exist.");
            }
        }

        public override Data.POCO.Category Get(int id)
        {
            try
            {
                return Context.Set<Data.POCO.Category>()
                    .Include(x => x.Posts)
                    .Single(x => x.Id == id);
            }
            catch
            {
                throw new IndexOutOfRangeException("Category doesn't exist.");
            }
        }

        public override IQueryable<Data.POCO.Category> GetAll()
        {
            return Context.Set<Data.POCO.Category>()
                .Include(x => x.Posts);
        }

        public async Task<bool> NameAlreadyExists(string name)
        {
            var category = await Context.Set<Data.POCO.Category>().Where(x => x.Name == name).FirstOrDefaultAsync();
            return category != null;
        }

        public override async Task<IQueryable<Data.POCO.Category>> GetAllAsync()
        {
            return Context.Set<Data.POCO.Category>()
                .Include(x => x.Posts);
        }
    }
}
