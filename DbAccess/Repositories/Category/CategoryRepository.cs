using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbAccess.DataContext;
using DbAccess.Specifications;
using DbAccess.Specifications.SortSpecification;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Repositories.Category
{
    public class CategoryRepository : Repository<Data.POCO.Category>, ICategoryRepository
    {
        public CategoryRepository(MyBlogContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Data.POCO.Category>> GetAsync(FilterSpecification<Data.POCO.Category> filterSpecification = null,
            PagingSpecification pagingSpecification = null,
            SortSpecification<Data.POCO.Category> sortSpecification = null)
        {
            IQueryable<Data.POCO.Category> query = Context.Set<Data.POCO.Category>();
            if (filterSpecification != null)
                query = query.Where(filterSpecification);

            if (sortSpecification != null)
                query = SortQuery(sortSpecification, query);

            if (pagingSpecification != null)
                query = query.Skip(pagingSpecification.Skip).Take(pagingSpecification.Take);

            return await query.Include(x => x.Posts).ToListAsync();
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

        public override IEnumerable<Data.POCO.Category> GetAll()
        {
            return Context.Set<Data.POCO.Category>()
                .Include(x => x.Posts).ToList();
        }

        public async Task<bool> NameAlreadyExists(string name)
        {
            var category = await Context.Set<Data.POCO.Category>().Where(x => x.Name == name).FirstOrDefaultAsync();
            return category != null;
        }

        public override async Task<IEnumerable<Data.POCO.Category>> GetAllAsync()
        {
            return await Context.Set<Data.POCO.Category>()
                .Include(x => x.Posts).ToListAsync();
        }
    }
}
