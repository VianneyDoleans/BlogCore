using AutoMapper;
using DbAccess.Repositories.Category;

namespace MyBlogAPI.DTO.Category.Converters
{
    public class CategoryIdConverter : ITypeConverter<int, DbAccess.Data.POCO.Category>
    {
        private readonly ICategoryRepository _repository;

        public CategoryIdConverter(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public DbAccess.Data.POCO.Category Convert(int source, DbAccess.Data.POCO.Category destination, ResolutionContext context)
        {
            return _repository.Get(source);
        }
    }
}
