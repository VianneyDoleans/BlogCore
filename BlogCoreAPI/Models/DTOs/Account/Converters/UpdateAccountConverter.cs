using AutoMapper;

namespace BlogCoreAPI.Models.DTOs.Account.Converters
{
    /// <summary>
    /// AutoMapper converter used to enable the conversion of <see cref="UpdateAccountDto"/> to <see cref="User"/>.
    /// </summary>
    public class UpdateAccountConverter : ITypeConverter<UpdateAccountDto, DBAccess.Data.User>
    {
        /// <inheritdoc />
        public DBAccess.Data.User Convert(UpdateAccountDto source, DBAccess.Data.User destination,
            ResolutionContext context)
        {
            destination.ProfilePictureUrl = string.IsNullOrEmpty(source.ProfilePictureUrl) ? null : source.ProfilePictureUrl;
            destination.UserDescription = source.UserDescription;
            destination.Email = source.Email;
            destination.UserName = source.UserName;
            return destination;
        }
    }
}
