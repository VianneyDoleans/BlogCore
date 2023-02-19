using AutoMapper;
using DBAccess.Repositories.Role;

namespace BlogCoreAPI.Models.DTOs.Role.Converters
{
    /// <summary>
    /// AutoMapper converter used to enable the conversion of <see cref="Role"/> to its resource Id.
    /// </summary>
    public class RoleIdConverter : ITypeConverter<int, DBAccess.Data.Role>
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
        public DBAccess.Data.Role Convert(int source, DBAccess.Data.Role destination, ResolutionContext context)
        {
            return _repository.Get(source);
        }
    }
}
