using AutoMapper;
using DbAccess.Repositories.User;

namespace MyBlogAPI.DTOs.User.Converters
{
    /// <summary>
    /// AutoMapper converter used to enable the conversion of <see cref="DbAccess.Data.POCO.User"/> to its resource Id.
    /// </summary>
    public class UserIdConverter : ITypeConverter<int, DbAccess.Data.POCO.User>
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
        public DbAccess.Data.POCO.User Convert(int source, DbAccess.Data.POCO.User destination,
            ResolutionContext context)
        {
            return _repository.Get(source);
        }
    }
}
