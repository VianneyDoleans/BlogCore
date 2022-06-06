using AutoMapper;
using DbAccess.Repositories.Post;

namespace MyBlogAPI.DTOs.Post.Converters
{
    /// <summary>
    /// AutoMapper converter used to enable the conversion of <see cref="DbAccess.Data.POCO.Post"/> to its resource Id.
    /// </summary>
    public class PostIdConverter : ITypeConverter<int, DbAccess.Data.POCO.Post>
    {
        private readonly IPostRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostIdConverter"/> class.
        /// </summary>
        /// <param name="repository"></param>
        public PostIdConverter(IPostRepository repository)
        {
            _repository = repository;
        }

        /// <inheritdoc />
        public DbAccess.Data.POCO.Post Convert(int source, DbAccess.Data.POCO.Post destination, ResolutionContext context)
        {
            return _repository.Get(source);
        }
    }
}
