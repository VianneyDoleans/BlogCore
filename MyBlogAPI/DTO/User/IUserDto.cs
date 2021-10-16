
namespace MyBlogAPI.DTO.User
{

    /// <summary>
    /// Interface of User Dto containing all the common properties of User Dto Type (GET, ADD, UPDATE).
    /// </summary>
    public interface IUserDto
    {
        public string Username { get; set; }

        public string EmailAddress { get; set; }

        public string Password { get; set; }

        public string UserDescription { get; set; }
    }
}
