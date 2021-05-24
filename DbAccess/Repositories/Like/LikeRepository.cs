using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbAccess.DataContext;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Repositories.Like
{
    public class LikeRepository : Repository<Data.POCO.Like>, ILikeRepository
    {
        public LikeRepository(MyBlogContext context) : base(context)
        {
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

        public override IQueryable<Data.POCO.Like> GetAll()
        {
            return Context.Set<Data.POCO.Like>().Include(x => x.User)
                .Include(x => x.Comment)
                .Include(x => x.Post);
        }

        public override async Task<IQueryable<Data.POCO.Like>> GetAllAsync()
        {
            return Context.Set<Data.POCO.Like>().Include(x => x.User)
                .Include(x => x.Comment)
                .Include(x => x.Post);
        }

        public async Task<IQueryable<Data.POCO.Like>> GetLikesFromPost(int id)
        {
            return Context.Set<Data.POCO.Like>()
                    .Include(x => x.Post)
                    .Include(x => x.User)
                    .Where(x => x.Post.Id == id);
        }

        public async Task<IQueryable<Data.POCO.Like>> GetLikesFromUser(int id)
        {
            return Context.Set<Data.POCO.Like>().Include(x => x.Comment)
                .Include(x => x.Post)
                .Include(x => x.User)
                .Where(x => x.User.Id == id);
        }

        public async Task<IQueryable<Data.POCO.Like>> GetLikesFromComment(int id)
        {
            return Context.Set<Data.POCO.Like>()
                .Include(x => x.User)
                .Include(x => x.Comment)
                .Where(x => x.Comment.Id == id);
        }

        public async Task<bool> LikeAlreadyExists(Data.POCO.Like like)
        {
            var result = await Context.Set<Data.POCO.Like>().FirstOrDefaultAsync(x => x.User == like.User && 
                x.Comment == like.Comment &&
                x.LikeableType == like.LikeableType &&
                x.Post == like.Post);
            return result != null;
        }
    }
}
