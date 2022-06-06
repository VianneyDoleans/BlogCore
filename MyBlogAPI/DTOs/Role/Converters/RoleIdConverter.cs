using AutoMapper;
using DbAccess.Repositories.Role;

namespace MyBlogAPI.DTOs.Role.Converters
{
    /// <summary>
    /// AutoMapper converter used to enable the conversion of <see cref="DbAccess.Data.POCO.Role"/> to its resource Id.
    /// </summary>
    public class RoleIdConverter : ITypeConverter<int, DbAccess.Data.POCO.Role>
    {
        private readonly IRoleRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleIdConverter"/> class.
        /// </summary>
        /// <param name="repository"></param>
        public RoleIdConverter(IRoleRepository repository)
        {
            _repository = repository;
        }

        /// <inheritdoc />
        public DbAccess.Data.POCO.Role Convert(int source, DbAccess.Data.POCO.Role destination, ResolutionContext context)
        {
            return _repository.Get(source);
        }
    }
}
