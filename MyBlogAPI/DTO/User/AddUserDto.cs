namespace MyBlogAPI.DTO.User
{
    /// <summary>
    /// Add Dto type of <see cref="DbAccess.Data.POCO.User"/>.
    /// </summary>
    public class AddUserDto : IUserDto
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string UserDescription { get; set; }
    }
}
