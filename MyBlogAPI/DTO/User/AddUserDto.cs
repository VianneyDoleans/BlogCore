namespace MyBlogAPI.DTO.User
{
    /// <summary>
    /// Add Dto type of <see cref="DbAccess.Data.POCO.User"/>.
    /// </summary>
    public class AddUserDto : IUserDto
    {
        public string Username { get; set; }

        public string EmailAddress { get; set; }

        public string Password { get; set; }

        public string UserDescription { get; set; }
    }
}
