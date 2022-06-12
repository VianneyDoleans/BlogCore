using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBAccess.DataContext;
using DBAccess.Specifications;
using DBAccess.Specifications.FilterSpecifications;
using DBAccess.Specifications.SortSpecification;
using Microsoft.EntityFrameworkCore;

namespace DBAccess.Repositories.Comment
{
    public class CommentRepository : Repository<Data.POCO.Comment>, ICommentRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentRepository"/> class.
        /// </summary>
        /// <param name="context"></param>
        public CommentRepository(BlogCoreContext context) : base(context)
        {
        }

        /// <inheritdoc />
        public override async Task<IEnumerable<Data.POCO.Comment>> GetAsync(FilterSpecification<Data.POCO.Comment> filterSpecification = null,
            PagingSpecification pagingSpecification = null,
            SortSpecification<Data.POCO.Comment> sortSpecification = null)
        {
            var query = GenerateQuery(filterSpecification, pagingSpecification, sortSpecification);
            return await query.Include(x => x.Likes)
                .Include(x => x.PostParent)
                .Include(x => x.ChildrenComments)
                .Include(x => x.Author).ToListAsync();
        }

        /// <inheritdoc />
        public override async Task<Data.POCO.Comment> GetAsync(int id)
        {
            try
            {
                return await _context.Set<Data.POCO.Comment>()
                    .Include(x => x.Likes)
                    .Include(x => x.PostParent)
                    .Include(x => x.ChildrenComments)
                    .Include(x => x.Author).SingleAsync(x => x.Id == id);
            }
            catch
            {
                throw new IndexOutOfRangeException("Comment doesn't exist.");
            }
        }

        /// <inheritdoc />
        public override Data.POCO.Comment Get(int id)
        {
            try
            {
                return _context.Set<Data.POCO.Comment>()
                    .Include(x => x.Likes)
                    .Include(x => x.PostParent)
                    .Include(x => x.ChildrenComments)
                    .Include(x => x.Author).Single(x => x.Id == id);
            }
            catch
            {
                throw new IndexOutOfRangeException("Comment doesn't exist.");
            }
        }

        /// <inheritdoc />
        public override IEnumerable<Data.POCO.Comment> GetAll()
        {
            return _context.Set<Data.POCO.Comment>()
                .Include(x => x.Likes)
                .Include(x => x.PostParent)
                .Include(x => x.ChildrenComments)
                .Include(x => x.Author).ToList();
        }

        /// <inheritdoc />
        public override async Task<IEnumerable<Data.POCO.Comment>> GetAllAsync()
        {
            return await _context.Set<Data.POCO.Comment>()
                .Include(x => x.Likes)
                .Include(x => x.PostParent)
                .Include(x => x.ChildrenComments)
                .Include(x => x.Author).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Data.POCO.Comment>> GetCommentsFromPost(int id)
        {
            return await _context.Set<Data.POCO.Comment>()
                .Include(x => x.Likes)
                .Include(x => x.Author)
                .Include(x => x.ChildrenComments)
                .Include(x => x.PostParent).Where(x => x.PostParent.Id == id).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Data.POCO.Comment>> GetCommentsFromUser(int id)
        {
            return await _context.Set<Data.POCO.Comment>()
                .Include(x => x.Likes)
                .Include(x => x.Author)
                .Include(x => x.ChildrenComments)
                .Include(x => x.PostParent).Where(x => x.Author.Id == id).ToListAsync();
        }
    }
}
