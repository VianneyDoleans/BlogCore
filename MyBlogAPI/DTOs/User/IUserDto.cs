
namespace MyBlogAPI.DTOs.User
{

    /// <summary>
    /// Interface of User Dto containing all the common properties of User Dto Type (GET, ADD, UPDATE).
    /// </summary>
    public interface IUserDto
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string UserDescription { get; set; }
    }
}
