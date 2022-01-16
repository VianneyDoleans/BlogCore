using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbAccess.DataContext;
using DbAccess.Specifications;
using DbAccess.Specifications.FilterSpecifications;
using DbAccess.Specifications.SortSpecification;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Repositories.Comment
{
    public class CommentRepository : Repository<Data.POCO.Comment>, ICommentRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentRepository"/> class.
        /// </summary>
        /// <param name="context"></param>
        public CommentRepository(MyBlogContext context) : base(context)
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
                return await Context.Set<Data.POCO.Comment>()
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
                return Context.Set<Data.POCO.Comment>()
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
            return Context.Set<Data.POCO.Comment>()
                .Include(x => x.Likes)
                .Include(x => x.PostParent)
                .Include(x => x.ChildrenComments)
                .Include(x => x.Author).ToList();
        }

        /// <inheritdoc />
        public override async Task<IEnumerable<Data.POCO.Comment>> GetAllAsync()
        {
            return await Context.Set<Data.POCO.Comment>()
                .Include(x => x.Likes)
                .Include(x => x.PostParent)
                .Include(x => x.ChildrenComments)
                .Include(x => x.Author).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Data.POCO.Comment>> GetCommentsFromPost(int id)
        {
            return await Context.Set<Data.POCO.Comment>()
                .Include(x => x.Likes)
                .Include(x => x.Author)
                .Include(x => x.ChildrenComments)
                .Include(x => x.PostParent).Where(x => x.PostParent.Id == id).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Data.POCO.Comment>> GetCommentsFromUser(int id)
        {
            return await Context.Set<Data.POCO.Comment>()
                .Include(x => x.Likes)
                .Include(x => x.Author)
                .Include(x => x.ChildrenComments)
                .Include(x => x.PostParent).Where(x => x.Author.Id == id).ToListAsync();
        }
    }
}
