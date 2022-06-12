using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBAccess.DataContext;
using DBAccess.Specifications;
using DBAccess.Specifications.FilterSpecifications;
using DBAccess.Specifications.SortSpecification;
using Microsoft.EntityFrameworkCore;

namespace DBAccess.Repositories.Like
{
    public class LikeRepository : Repository<Data.POCO.Like>, ILikeRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LikeRepository"/> class.
        /// </summary>
        /// <param name="context"></param>
        public LikeRepository(MyBlogContext context) : base(context)
        {
        }

        /// <inheritdoc />
        public override async Task<IEnumerable<Data.POCO.Like>> GetAsync(FilterSpecification<Data.POCO.Like> filterSpecification = null,
            PagingSpecification pagingSpecification = null,
            SortSpecification<Data.POCO.Like> sortSpecification = null)
        {
            var query = GenerateQuery(filterSpecification, pagingSpecification, sortSpecification);
            return await query.Include(x => x.User)
                .Include(x => x.Comment)
                .Include(x => x.Post).ToListAsync();
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
        public override IEnumerable<Data.POCO.Like> GetAll()
        {
            return Context.Set<Data.POCO.Like>().Include(x => x.User)
                .Include(x => x.Comment)
                .Include(x => x.Post).ToList();
        }

        /// <inheritdoc />
        public override async Task<IEnumerable<Data.POCO.Like>> GetAllAsync()
        {
            return await Context.Set<Data.POCO.Like>().Include(x => x.User)
                .Include(x => x.Comment)
                .Include(x => x.Post).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Data.POCO.Like>> GetLikesFromPost(int id)
        {
            return await Context.Set<Data.POCO.Like>()
                    .Include(x => x.Post)
                    .Include(x => x.User)
                    .Where(x => x.Post.Id == id).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Data.POCO.Like>> GetLikesFromUser(int id)
        {
            return await Context.Set<Data.POCO.Like>().Include(x => x.Comment)
                .Include(x => x.Post)
                .Include(x => x.User)
                .Where(x => x.User.Id == id).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Data.POCO.Like>> GetLikesFromComment(int id)
        {
            return await Context.Set<Data.POCO.Like>()
                .Include(x => x.User)
                .Include(x => x.Comment)
                .Where(x => x.Comment.Id == id).ToListAsync();
        }

        /// <inheritdoc />
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
