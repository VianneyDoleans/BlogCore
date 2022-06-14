using AutoMapper;
using DBAccess.Data;
using DBAccess.Repositories.Like;

namespace BlogCoreAPI.DTOs.Like.Converters
{
    /// <summary>
    /// AutoMapper converter used to enable the conversion of <see cref="Like"/> to its resource Id.
    /// </summary>
    public class LikeIdConverter : ITypeConverter<int, DBAccess.Data.Like>
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
        public DBAccess.Data.Like Convert(int source, DBAccess.Data.Like destination, ResolutionContext context)
        {
            return _repository.Get(source);
        }
    }
}
