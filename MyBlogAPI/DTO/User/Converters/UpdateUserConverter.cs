using AutoMapper;

namespace MyBlogAPI.DTO.User.Converters
{
    public class UpdateUserConverter : ITypeConverter<UpdateUserDto, DbAccess.Data.POCO.User>
    {
        public DbAccess.Data.POCO.User Convert(UpdateUserDto source, DbAccess.Data.POCO.User destination,
            ResolutionContext context)
        {
            destination.UserDescription = source.UserDescription;
            destination.EmailAddress = source.EmailAddress;
            destination.Username = source.Username;
            destination.Password = source.Password;
            return destination;
        }
    }
}
