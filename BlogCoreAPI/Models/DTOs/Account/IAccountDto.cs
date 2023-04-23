namespace BlogCoreAPI.Models.DTOs.Account
{

    /// <summary>
    /// Interface of User Dto containing all the common properties of Account Dto Type (GET, ADD, UPDATE).
    /// </summary>
    public interface IAccountDto
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string ProfilePictureUrl { get; set; }

        public string UserDescription { get; set; }
    }
}
