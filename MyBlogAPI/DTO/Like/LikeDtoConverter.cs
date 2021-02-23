using AutoMapper;
using DbAccess.Repositories.Like;

namespace MyBlogAPI.DTO.Like
{
    public class LikeDtoConverter : ITypeConverter<int, DbAccess.Data.POCO.Like>
    {
        private readonly ILikeRepository _repository;

        public LikeDtoConverter(ILikeRepository repository)
        {
            _repository = repository;
        }

        public DbAccess.Data.POCO.Like Convert(int source, DbAccess.Data.POCO.Like destination, ResolutionContext context)
        {
            return _repository.Get(source);
        }
    }
}
