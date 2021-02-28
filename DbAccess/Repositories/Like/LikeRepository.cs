using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            return await Context.Set<Data.POCO.Like>().Include(x => x.User)
                .Include(x => x.Comment)
                .Include(x => x.Post)
                .SingleAsync(x => x.Id == id);
        }

        public override Data.POCO.Like Get(int id)
        {
            return Context.Set<Data.POCO.Like>().Include(x => x.User)
                .Include(x => x.Comment)
                .Include(x => x.Post)
                .Single(x => x.Id == id);
        }

        public override IEnumerable<Data.POCO.Like> GetAll()
        {
            return Context.Set<Data.POCO.Like>().Include(x => x.User)
                .Include(x => x.Comment)
                .Include(x => x.Post)
                .ToList();
        }

        public override async Task<IEnumerable<Data.POCO.Like>> GetAllAsync()
        {
            return await Context.Set<Data.POCO.Like>().Include(x => x.User)
                .Include(x => x.Comment)
                .Include(x => x.Post)
                .ToListAsync();
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
            var result = await Context.Set<Data.POCO.Like>().FirstOrDefaultAsync(x => x.User == like.User && 
                x.Comment == like.Comment &&
                x.LikeableType == like.LikeableType &&
                x.Post == like.Post);
            return result != null;
        }
    }
}
