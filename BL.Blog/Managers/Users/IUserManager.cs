using Blog.BL.DTOs.Users;

namespace Blog.BL.Managers.Users;

public interface IUserManager
{
    Task<TokenRespnseDTO> Registration(RegistrationDTO registration);
    Task<TokenRespnseDTO> Login(LoginDTO userData);
}
