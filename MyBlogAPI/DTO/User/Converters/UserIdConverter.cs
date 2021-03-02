using AutoMapper;
using DbAccess.Repositories.User;

namespace MyBlogAPI.DTO.User.Converters
{
    public class UserIdConverter : ITypeConverter<int, DbAccess.Data.POCO.User>
    {
        private readonly IUserRepository _repository;

        public UserIdConverter(IUserRepository repository)
        {
            _repository = repository;
        }

        public DbAccess.Data.POCO.User Convert(int source, DbAccess.Data.POCO.User destination,
            ResolutionContext context)
        {
            return _repository.Get(source);
        }
    }
}
