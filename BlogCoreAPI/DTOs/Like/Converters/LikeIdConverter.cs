using AutoMapper;
using DBAccess.Data.POCO;
using DBAccess.Repositories.Like;

namespace BlogCoreAPI.DTOs.Like.Converters
{
    /// <summary>
    /// AutoMapper converter used to enable the conversion of <see cref="Like"/> to its resource Id.
    /// </summary>
    public class LikeIdConverter : ITypeConverter<int, DBAccess.Data.POCO.Like>
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
        public DBAccess.Data.POCO.Like Convert(int source, DBAccess.Data.POCO.Like destination, ResolutionContext context)
        {
            return _repository.Get(source);
        }
    }
}
