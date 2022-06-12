using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DBAccess.DataContext;
using DBAccess.Specifications;
using DBAccess.Specifications.FilterSpecifications;
using DBAccess.Specifications.SortSpecification;
using Microsoft.EntityFrameworkCore;

namespace DBAccess.Repositories.Post
{
    public class PostRepository : Repository<Data.POCO.Post>, IPostRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostRepository"/> class.
        /// </summary>
        /// <param name="context"></param>
        public PostRepository(MyBlogContext context) : base(context)
        {
        }

        /// <inheritdoc />
        public override async Task<IEnumerable<Data.POCO.Post>> GetAsync(FilterSpecification<Data.POCO.Post> filterSpecification = null,
            PagingSpecification pagingSpecification = null,
            SortSpecification<Data.POCO.Post> sortSpecification = null)
        {
            var query = GenerateQuery(filterSpecification, pagingSpecification, sortSpecification);
            return await query.Include(x => x.Likes)
                .Include(x => x.Author)
                .Include(x => x.PostTags)
                .Include(x => x.Category).ToListAsync();
        }

        /// <inheritdoc />
        public override async Task<Data.POCO.Post> GetAsync(int id)
        {
            try
            {
                return await Context.Set<Data.POCO.Post>().Include(x => x.Likes)
                    .Include(x => x.Author)
                    .Include(x => x.PostTags)
                    .Include(x => x.Category)
                    .SingleAsync(x => x.Id == id);
            }
            catch
            {
                throw new IndexOutOfRangeException("Post doesn't exist.");
            }
        }

        /// <inheritdoc />
        public override Data.POCO.Post Get(int id)
        {
            try
            {
                return Context.Set<Data.POCO.Post>().Include(x => x.Likes)
                    .Include(x => x.Author)
                    .Include(x => x.PostTags)
                    .Include(x => x.Category)
                    .Single(x => x.Id == id);
            }
            catch
            {
                throw new IndexOutOfRangeException("Post doesn't exist.");
            }
        }

        /// <inheritdoc />
        public override IEnumerable<Data.POCO.Post> GetAll()
        {
            return Context.Set<Data.POCO.Post>().Include(x => x.Likes)
                .Include(x => x.Author)
                .Include(x => x.PostTags)
                .Include(x => x.Category).ToList();
        }

        /// <inheritdoc />
        public override async Task<IEnumerable<Data.POCO.Post>> GetAllAsync()
        {
            return await Context.Set<Data.POCO.Post>().Include(x => x.Likes)
                .Include(x => x.Author)
                .Include(x => x.PostTags)
                .Include(x => x.Category).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Data.POCO.Post>> GetPostsFromUser(int id)
        {
            return await Context.Set<Data.POCO.Post>().Include(x => x.Likes)
                .Include(x => x.PostTags)
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Where(x => x.Author.Id == id).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Data.POCO.Post>> GetPostsFromTag(int id)
        {
            return await Context.Set<Data.POCO.JoiningEntity.PostTag>().Include(x => x.Post.Likes)
                .Include(x => x.Post.Author)
                .Include(x => x.Post.Category)
                .Include(x => x.Tag)
                .Where(x => x.TagId == id).Select(x => x.Post).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Data.POCO.Post>> GetPostsFromCategory(int id)
        {
            return await Context.Set<Data.POCO.Post>()
                .Include(x => x.Likes)
                .Include(x => x.Author)
                .Include(x => x.PostTags)
                .Include(x => x.Category)
                .Where(x => x.Category.Id == id).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<bool> NameAlreadyExists(string name)
        {
            var post = await Context.Set<Data.POCO.Post>().Where(x => x.Name == name).FirstOrDefaultAsync();
            return post != null;
        }
    }
}
