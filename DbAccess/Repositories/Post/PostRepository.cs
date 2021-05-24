using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbAccess.DataContext;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Repositories.Post
{
    public class PostRepository : Repository<Data.POCO.Post>, IPostRepository
    {
        public PostRepository(MyBlogContext context) : base(context)
        {
        }

        public override async Task<Data.POCO.Post> GetAsync(int id)
        {
            try
            {
                return await Context.Set<Data.POCO.Post>().Include(x => x.Author)
                    .Include(x => x.PostTags)
                    .Include(x => x.Category)
                    .SingleAsync(x => x.Id == id);
            }
            catch
            {
                throw new IndexOutOfRangeException("Post doesn't exist.");
            }
        }

        public override Data.POCO.Post Get(int id)
        {
            try
            {
                return Context.Set<Data.POCO.Post>().Include(x => x.Author)
                    .Include(x => x.PostTags)
                    .Include(x => x.Category)
                    .Single(x => x.Id == id);
            }
            catch
            {
                throw new IndexOutOfRangeException("Post doesn't exist.");
            }
        }

        public override IQueryable<Data.POCO.Post> GetAll()
        {
            return Context.Set<Data.POCO.Post>()
                .Include(x => x.Author)
                .Include(x => x.PostTags)
                .Include(x => x.Category);
        }

        public override async Task<IQueryable<Data.POCO.Post>> GetAllAsync()
        {
            return Context.Set<Data.POCO.Post>()
                .Include(x => x.Author)
                .Include(x => x.PostTags)
                .Include(x => x.Category);
        }

        public async Task<IQueryable<Data.POCO.Post>> GetPostsFromUser(int id)
        {
            return Context.Set<Data.POCO.Post>().Include(x => x.PostTags)
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Where(x => x.Author.Id == id);
        }

        public async Task<IQueryable<Data.POCO.Post>> GetPostsFromTag(int id)
        {
            return Context.Set<Data.POCO.JoiningEntity.PostTag>().Include(x => x.Post.Author)
                .Include(x => x.Post.Category)
                .Include(x => x.Tag)
                .Where(x => x.TagId == id).Select(x => x.Post);
        }

        public async Task<IQueryable<Data.POCO.Post>> GetPostsFromCategory(int id)
        {
            return Context.Set<Data.POCO.Post>()
                .Include(x => x.Author)
                .Include(x => x.PostTags)
                .Include(x => x.Category)
                .Where(x => x.Category.Id == id);
        }

        public async Task<bool> NameAlreadyExists(string name)
        {
            var post = await Context.Set<Data.POCO.Post>().Where(x => x.Name == name).FirstOrDefaultAsync();
            return post != null;
        }
    }
}
