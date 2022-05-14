using AutoMapper;
using DbAccess.Repositories.Category;

namespace MyBlogAPI.DTO.Category.Converters
{
    /// <summary>
    /// AutoMapper converter used to enable the conversion of <see cref="DbAccess.Data.POCO.Category"/> to its resource Id.
    /// </summary>
    public class CategoryIdConverter : ITypeConverter<int, DbAccess.Data.POCO.Category>
    {
        private readonly ICategoryRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryIdConverter"/> class.
        /// </summary>
        /// <param name="repository"></param>
        public CategoryIdConverter(ICategoryRepository repository)
        {
            _repository = repository;
        }

        /// <inheritdoc />
        public DbAccess.Data.POCO.Category Convert(int source, DbAccess.Data.POCO.Category destination, ResolutionContext context)
        {
            return _repository.Get(source);
        }
    }
}
