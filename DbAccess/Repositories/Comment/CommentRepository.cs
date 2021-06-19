using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbAccess.DataContext;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Repositories.Comment
{
    public class CommentRepository : Repository<Data.POCO.Comment>, ICommentRepository
    {
        public CommentRepository(MyBlogContext context) : base(context)
        {
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

        public override IQueryable<Data.POCO.Comment> GetAll()
        {
            return Context.Set<Data.POCO.Comment>()
                .Include(x => x.PostParent)
                .Include(x => x.Author);
        }

        public override async Task<IEnumerable<Data.POCO.Comment>> GetAllAsync()
        {
            return Context.Set<Data.POCO.Comment>()
                .Include(x => x.PostParent)
                .Include(x => x.Author);
        }

        public async Task<IEnumerable<Data.POCO.Comment>> GetCommentsFromPost(int id)
        {
            return Context.Set<Data.POCO.Comment>()
                .Include(x => x.Author)
                .Include(x => x.PostParent).Where(x => x.PostParent.Id == id);
        }

        public async Task<IEnumerable<Data.POCO.Comment>> GetCommentsFromUser(int id)
        {
            return Context.Set<Data.POCO.Comment>()
                .Include(x => x.Author)
                .Include(x => x.PostParent).Where(x => x.Author.Id == id);
        }
    }
}
