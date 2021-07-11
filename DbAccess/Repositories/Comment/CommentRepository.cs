using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbAccess.DataContext;
using DbAccess.Specifications;
using DbAccess.Specifications.SortSpecification;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Repositories.Comment
{
    public class CommentRepository : Repository<Data.POCO.Comment>, ICommentRepository
    {
        public CommentRepository(MyBlogContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Data.POCO.Comment>> GetAsync(FilterSpecification<Data.POCO.Comment> filterSpecification = null,
            PagingSpecification pagingSpecification = null,
            SortSpecification<Data.POCO.Comment> sortSpecification = null)
        {
            IQueryable<Data.POCO.Comment> query = Context.Set<Data.POCO.Comment>();
            if (filterSpecification != null)
                query = query.Where(filterSpecification);

            if (sortSpecification != null)
                query = SortQuery(sortSpecification, query);

            if (pagingSpecification != null)
                query = query.Skip(pagingSpecification.Skip).Take(pagingSpecification.Take);

            return await query.Include(x => x.PostParent)
                .Include(x => x.Author).ToListAsync();
        }

        public override async Task<Data.POCO.Comment> GetAsync(int id)
        {
            try
            {
                return await Context.Set<Data.POCO.Comment>()
                    .Include(x => x.PostParent)
                    .Include(x => x.Author).SingleAsync(x => x.Id == id);
            }
            catch
            {
                throw new IndexOutOfRangeException("Comment doesn't exist.");
            }
        }

        public override Data.POCO.Comment Get(int id)
        {
            try
            {
                return Context.Set<Data.POCO.Comment>()
                    .Include(x => x.PostParent)
                    .Include(x => x.Author).Single(x => x.Id == id);
            }
            catch
            {
                throw new IndexOutOfRangeException("Comment doesn't exist.");
            }
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
