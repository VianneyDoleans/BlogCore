using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBAccess.DataContext;
using DBAccess.Specifications;
using DBAccess.Specifications.FilterSpecifications;
using DBAccess.Specifications.SortSpecification;
using Microsoft.EntityFrameworkCore;

namespace DBAccess.Repositories.Category
{
    /// <summary>
    /// Repository used to manipulate <see cref="Data.POCO.Category"/> from database (CRUD and more).
    /// </summary>
    public class CategoryRepository : Repository<Data.POCO.Category>, ICategoryRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryRepository"/> class.
        /// </summary>
        /// <param name="context"></param>
        public CategoryRepository(BlogCoreContext context) : base(context)
        {
        }

        /// <inheritdoc />
        public override async Task<IEnumerable<Data.POCO.Category>> GetAsync(FilterSpecification<Data.POCO.Category> filterSpecification = null,
            PagingSpecification pagingSpecification = null,
            SortSpecification<Data.POCO.Category> sortSpecification = null)
        {
            var query = GenerateQuery(filterSpecification, pagingSpecification, sortSpecification);
            return await query.Include(x => x.Posts).ToListAsync();
        }

        /// <inheritdoc />
        public override async Task<Data.POCO.Category> GetAsync(int id)
        {
            try
            {
                return await context.Set<Data.POCO.Category>()
                    .Include(x => x.Posts)
                    .SingleAsync(x => x.Id == id);
            }
            catch
            {
                throw new IndexOutOfRangeException("Category doesn't exist.");
            }
        }

        /// <inheritdoc />
        public override Data.POCO.Category Get(int id)
        {
            try
            {
                return context.Set<Data.POCO.Category>()
                    .Include(x => x.Posts)
                    .Single(x => x.Id == id);
            }
            catch
            {
                throw new IndexOutOfRangeException("Category doesn't exist.");
            }
        }

        /// <inheritdoc />
        public override IEnumerable<Data.POCO.Category> GetAll()
        {
            return context.Set<Data.POCO.Category>()
                .Include(x => x.Posts).ToList();
        }

        /// <inheritdoc />
        public async Task<bool> NameAlreadyExists(string name)
        {
            var category = await context.Set<Data.POCO.Category>().Where(x => x.Name == name).FirstOrDefaultAsync();
            return category != null;
        }

        /// <inheritdoc />
        public override async Task<IEnumerable<Data.POCO.Category>> GetAllAsync()
        {
            return await context.Set<Data.POCO.Category>()
                .Include(x => x.Posts).ToListAsync();
        }
    }
}
