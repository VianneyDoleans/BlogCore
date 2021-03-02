using AutoMapper;
using DbAccess.Repositories.Like;

namespace MyBlogAPI.DTO.Like.Converters
{
    public class LikeIdConverter : ITypeConverter<int, DbAccess.Data.POCO.Like>
    {
        private readonly ILikeRepository _repository;

        public LikeIdConverter(ILikeRepository repository)
        {
            _repository = repository;
        }

        public DbAccess.Data.POCO.Like Convert(int source, DbAccess.Data.POCO.Like destination, ResolutionContext context)
        {
            return _repository.Get(source);
        }
    }
}
