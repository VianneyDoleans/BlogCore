using AutoMapper;

namespace BlogCoreAPI.Models.DTOs.User.Converters
{
    /// <summary>
    /// AutoMapper converter used to enable the conversion of <see cref="UpdateUserDto"/> to <see cref="User"/>.
    /// </summary>
    public class UpdateUserConverter : ITypeConverter<UpdateUserDto, DBAccess.Data.User>
    {
        /// <inheritdoc />
        public DBAccess.Data.User Convert(UpdateUserDto source, DBAccess.Data.User destination,
            ResolutionContext context)
        {
            destination.UserDescription = source.UserDescription;
            destination.Email = source.Email;
            destination.UserName = source.UserName;
            destination.Password = source.Password;
            return destination;
        }
    }
}
