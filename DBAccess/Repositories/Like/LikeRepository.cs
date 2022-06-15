using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBAccess.DataContext;
using DBAccess.Exceptions;
using DBAccess.Specifications;
using DBAccess.Specifications.FilterSpecifications;
using DBAccess.Specifications.SortSpecification;
using Microsoft.EntityFrameworkCore;

namespace DBAccess.Repositories.Like
{
    public class LikeRepository : Repository<Data.Like>, ILikeRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LikeRepository"/> class.
        /// </summary>
        /// <param name="context"></param>
        public LikeRepository(BlogCoreContext context) : base(context)
        {
        }

        /// <inheritdoc />
        public override async Task<IEnumerable<Data.Like>> GetAsync(FilterSpecification<Data.Like> filterSpecification = null,
            PagingSpecification pagingSpecification = null,
            SortSpecification<Data.Like> sortSpecification = null)
        {
            var query = GenerateQuery(filterSpecification, pagingSpecification, sortSpecification);
            return await query.Include(x => x.User)
                .Include(x => x.Comment)
                .Include(x => x.Post).ToListAsync();
        }

        /// <inheritdoc />
        public override async Task<Data.Like> GetAsync(int id)
        {
            try
            {
                return await _context.Set<Data.Like>().Include(x => x.User)
                    .Include(x => x.Comment)
                    .Include(x => x.Post)
                    .SingleAsync(x => x.Id == id);
            }
            catch
            {
                throw new ResourceNotFoundException("Like doesn't exist.");
            }
        }

        /// <inheritdoc />
        public override Data.Like Get(int id)
        {
            try
            {
                return _context.Set<Data.Like>().Include(x => x.User)
                    .Include(x => x.Comment)
                    .Include(x => x.Post)
                    .Single(x => x.Id == id);
            }
            catch
            {
                throw new ResourceNotFoundException("Like doesn't exist.");
            }
        }

        /// <inheritdoc />
        public override IEnumerable<Data.Like> GetAll()
        {
            return _context.Set<Data.Like>().Include(x => x.User)
                .Include(x => x.Comment)
                .Include(x => x.Post).ToList();
        }

        /// <inheritdoc />
        public override async Task<IEnumerable<Data.Like>> GetAllAsync()
        {
            return await _context.Set<Data.Like>().Include(x => x.User)
                .Include(x => x.Comment)
                .Include(x => x.Post).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Data.Like>> GetLikesFromPost(int id)
        {
            return await _context.Set<Data.Like>()
                    .Include(x => x.Post)
                    .Include(x => x.User)
                    .Where(x => x.Post.Id == id).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Data.Like>> GetLikesFromUser(int id)
        {
            return await _context.Set<Data.Like>().Include(x => x.Comment)
                .Include(x => x.Post)
                .Include(x => x.User)
                .Where(x => x.User.Id == id).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Data.Like>> GetLikesFromComment(int id)
        {
            return await _context.Set<Data.Like>()
                .Include(x => x.User)
                .Include(x => x.Comment)
                .Where(x => x.Comment.Id == id).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<bool> LikeAlreadyExists(Data.Like like)
        {
            if (like == null)
                return false;
            var result = await _context.Set<Data.Like>().FirstOrDefaultAsync(x => x.User == like.User && 
                                                                             x.Comment == like.Comment &&
                                                                             x.LikeableType == like.LikeableType &&
                                                                             x.Post == like.Post);
            return result != null;
        }
    }
}
