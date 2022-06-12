using AutoMapper;
using DBAccess.Data.POCO;
using DBAccess.Repositories.Role;

namespace BlogCoreAPI.DTOs.Role.Converters
{
    /// <summary>
    /// AutoMapper converter used to enable the conversion of <see cref="Role"/> to its resource Id.
    /// </summary>
    public class RoleIdConverter : ITypeConverter<int, DBAccess.Data.POCO.Role>
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
        public DBAccess.Data.POCO.Role Convert(int source, DBAccess.Data.POCO.Role destination, ResolutionContext context)
        {
            return _repository.Get(source);
        }
    }
}
