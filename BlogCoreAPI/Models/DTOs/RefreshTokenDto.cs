#nullable enable
namespace BlogCoreAPI.Models.DTOs;

public class RefreshTokenDto
{
    public int UserId { get; set; }

    public string? RefreshToken { get; set; }
}