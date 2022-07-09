namespace BlogCoreAPI.DTOs.User
{
    /// <summary>
    /// UPDATE Dto type of <see cref="User"/>.
    /// </summary>
    public class UpdateUserDto : ADto, IUserDto
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string UserDescription { get; set; }
    }
}
