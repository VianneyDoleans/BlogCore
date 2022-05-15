using AutoMapper;
using DbAccess.Repositories.Tag;

namespace MyBlogAPI.DTOs.Tag.Converters
{
    /// <summary>
    /// AutoMapper converter used to enable the conversion of <see cref="DbAccess.Data.POCO.Tag"/> to its resource Id.
    /// </summary>
    public class TagIdConverter : ITypeConverter<int, DbAccess.Data.POCO.Tag>
    {
        private readonly ITagRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagIdConverter"/> class.
        /// </summary>
        /// <param name="repository"></param>
        public TagIdConverter(ITagRepository repository)
        {
            _repository = repository;
        }

        /// <inheritdoc />
        public DbAccess.Data.POCO.Tag Convert(int source, DbAccess.Data.POCO.Tag destination, ResolutionContext context)
        {
            return _repository.Get(source);
        }
    }
}
