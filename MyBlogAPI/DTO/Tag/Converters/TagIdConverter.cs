using AutoMapper;
using DbAccess.Repositories.Tag;

namespace MyBlogAPI.DTO.Tag.Converters
{
    public class TagIdConverter : ITypeConverter<int, DbAccess.Data.POCO.Tag>
    {
        private readonly ITagRepository _repository;

        public TagIdConverter(ITagRepository repository)
        {
            _repository = repository;
        }

        public DbAccess.Data.POCO.Tag Convert(int source, DbAccess.Data.POCO.Tag destination, ResolutionContext context)
        {
            return _repository.Get(source);
        }
    }
}
