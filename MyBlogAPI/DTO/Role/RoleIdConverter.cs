using AutoMapper;
using DbAccess.Repositories.Role;

namespace MyBlogAPI.DTO.Role
{
    public class RoleIdConverter : ITypeConverter<int, DbAccess.Data.POCO.Role>
    {
        private readonly IRoleRepository _repository;

        public RoleIdConverter(IRoleRepository repository)
        {
            _repository = repository;
        }

        public DbAccess.Data.POCO.Role Convert(int source, DbAccess.Data.POCO.Role destination, ResolutionContext context)
        {
            return _repository.Get(source);
        }
    }
}
