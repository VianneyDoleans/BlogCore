using AutoMapper;
using DBAccess.Data.POCO;
using DBAccess.Repositories.User;

namespace BlogCoreAPI.DTOs.User.Converters
{
    /// <summary>
    /// AutoMapper converter used to enable the conversion of <see cref="User"/> to its resource Id.
    /// </summary>
    public class UserIdConverter : ITypeConverter<int, DBAccess.Data.POCO.User>
    {
        private readonly IUserRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserIdConverter"/> class.
        /// </summary>
        /// <param name="repository"></param>
        public UserIdConverter(IUserRepository repository)
        {
            _repository = repository;
        }

        /// <inheritdoc />
        public DBAccess.Data.POCO.User Convert(int source, DBAccess.Data.POCO.User destination,
            ResolutionContext context)
        {
            return _repository.Get(source);
        }
    }
}
