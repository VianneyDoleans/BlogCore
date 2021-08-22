using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbAccess.DataContext;
using DbAccess.Specifications;
using DbAccess.Specifications.FilterSpecifications;
using DbAccess.Specifications.SortSpecification;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Repositories.Like
{
    public class LikeRepository : Repository<Data.POCO.Like>, ILikeRepository
    {
        public LikeRepository(MyBlogContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Data.POCO.Like>> GetAsync(FilterSpecification<Data.POCO.Like> filterSpecification = null,
            PagingSpecification pagingSpecification = null,
            SortSpecification<Data.POCO.Like> sortSpecification = null)
        {
            IQueryable<Data.POCO.Like> query = Context.Set<Data.POCO.Like>();
            if (filterSpecification != null)
                query = query.Where(filterSpecification);

            if (sortSpecification != null)
                query = SortQuery(sortSpecification, query);

            if (pagingSpecification != null)
                query = query.Skip(pagingSpecification.Skip).Take(pagingSpecification.Take);

            return await query.Include(x => x.User)
                .Include(x => x.Comment)
                .Include(x => x.Post).ToListAsync();
        }

        public override async Task<Data.POCO.Like> GetAsync(int id)
        {
            try
            {
                return await Context.Set<Data.POCO.Like>().Include(x => x.User)
                    .Include(x => x.Comment)
                    .Include(x => x.Post)
                    .SingleAsync(x => x.Id == id);
            }
            catch
            {
                throw new IndexOutOfRangeException("Like doesn't exist.");
            }
        }

        public override Data.POCO.Like Get(int id)
        {
            try
            {
                return Context.Set<Data.POCO.Like>().Include(x => x.User)
                    .Include(x => x.Comment)
                    .Include(x => x.Post)
                    .Single(x => x.Id == id);
            }
            catch
            {
                throw new IndexOutOfRangeException("Like doesn't exist.");
            }
        }

        public override IEnumerable<Data.POCO.Like> GetAll()
        {
            return Context.Set<Data.POCO.Like>().Include(x => x.User)
                .Include(x => x.Comment)
                .Include(x => x.Post).ToList();
        }

        public override async Task<IEnumerable<Data.POCO.Like>> GetAllAsync()
        {
            return await Context.Set<Data.POCO.Like>().Include(x => x.User)
                .Include(x => x.Comment)
                .Include(x => x.Post).ToListAsync();
        }

        public async Task<IEnumerable<Data.POCO.Like>> GetLikesFromPost(int id)
        {
            return await Context.Set<Data.POCO.Like>()
                    .Include(x => x.Post)
                    .Include(x => x.User)
                    .Where(x => x.Post.Id == id).ToListAsync();
        }

        public async Task<IEnumerable<Data.POCO.Like>> GetLikesFromUser(int id)
        {
            return await Context.Set<Data.POCO.Like>().Include(x => x.Comment)
                .Include(x => x.Post)
                .Include(x => x.User)
                .Where(x => x.User.Id == id).ToListAsync();
        }

        public async Task<IEnumerable<Data.POCO.Like>> GetLikesFromComment(int id)
        {
            return await Context.Set<Data.POCO.Like>()
                .Include(x => x.User)
                .Include(x => x.Comment)
                .Where(x => x.Comment.Id == id).ToListAsync();
        }

        public async Task<bool> LikeAlreadyExists(Data.POCO.Like like)
        {
            if (like == null)
                return false;
            var result = await Context.Set<Data.POCO.Like>().FirstOrDefaultAsync(x => x.User == like.User && 
                x.Comment == like.Comment &&
                x.LikeableType == like.LikeableType &&
                x.Post == like.Post);
            return result != null;
        }
    }
}
