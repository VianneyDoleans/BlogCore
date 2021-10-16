using AutoMapper;
using DbAccess.Repositories.Like;

namespace MyBlogAPI.DTO.Like.Converters
{
    /// <summary>
    /// AutoMapper converter used to enable the conversion of <see cref="DbAccess.Data.POCO.Like"/> to its resource Id.
    /// </summary>
    public class LikeIdConverter : ITypeConverter<int, DbAccess.Data.POCO.Like>
    {
        private readonly ILikeRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="LikeIdConverter"/> class.
        /// </summary>
        /// <param name="repository"></param>
        public LikeIdConverter(ILikeRepository repository)
        {
            _repository = repository;
        }

        /// <inheritdoc />
        public DbAccess.Data.POCO.Like Convert(int source, DbAccess.Data.POCO.Like destination, ResolutionContext context)
        {
            return _repository.Get(source);
        }
    }
}
