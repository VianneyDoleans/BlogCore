
namespace MyBlogAPI.DTO.User
{
    internal interface IUserDto
    {
        public string Username { get; set; }

        public string EmailAddress { get; set; }

        public string UserDescription { get; set; }
    }
}
