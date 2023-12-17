using Blog.DAL.DataTypes;

namespace Blog.BL.DTOs.Users;

public record RegistrationDTO(string DisplayName, string UserName, Gender Gender, string Email, string Password);

