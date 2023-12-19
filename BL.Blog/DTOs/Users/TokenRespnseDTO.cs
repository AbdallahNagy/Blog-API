using System.Text.Json.Serialization;

namespace Blog.BL.DTOs.Users;

public record TokenRespnseDTO
{
    public string? Token { get; }

    [JsonIgnore]
    public string? RefreshToken { get; }
    public DateTime RefreshTokenExpiration { get; }

    public TokenRespnseDTO(string? token)
    {
        Token = token;
    }

    public TokenRespnseDTO(string? token, DateTime refreshTokenExpiration)
    {
        Token = token;
        RefreshTokenExpiration = refreshTokenExpiration;
    }
};
