using Blog.BL.DTOs.Users;
using Blog.BL.Exception_Handling;
using Blog.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Blog.BL.Managers.Users;

public class BlogUserManager : IUserManager
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;

    public BlogUserManager(UserManager<User> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<TokenRespnseDTO> Registration(RegistrationDTO userData)
    {
		var userExist = await _userManager.FindByEmailAsync(userData.Email);

        if (userExist != null) throw new BusinessException(404, "Email Already Exist.");

        // create user 
        var newUser = new User()
        {
            Email = userData.Email,
            UserName = userData.UserName,
            DisplayName = userData.DisplayName,
            Gender = userData.Gender
        };

        var isUserCreated = await _userManager.CreateAsync(newUser, userData.Password);

        var errors = isUserCreated.Errors.Select(e => e.Description).ToList();
        var errorsSerialized = new StringBuilder();

        for (int i = 0; i < errors.Count; i++)
        {
            errorsSerialized.Append($"[{i+1}] {errors[i]} ");
        }

        if (!isUserCreated.Succeeded) throw new BusinessException(500, "Internal Server Error: Couldn't create user.", errorsSerialized.ToString());

        // generate user token
        var token = GenerateJWTToken(newUser);

        return await Task.FromResult(new TokenRespnseDTO(token));
    }

    public async Task<TokenRespnseDTO> Login(LoginDTO userData)
    {
        var existingUser = await _userManager.FindByEmailAsync(userData.Email)
            ?? throw new BusinessException(404, "Invalid Email or Passwod");

        var isCorrect = await _userManager.CheckPasswordAsync(existingUser, userData.Password);

        if (!isCorrect) throw new BusinessException(404, "Invalid Email or Passwod");

        var token = GenerateJWTToken(existingUser);

        return await Task.FromResult(new TokenRespnseDTO(token));
    }

    private string GenerateJWTToken(User user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JWTConfig:SecretKey").Value ?? "ld2e5nvi1adkq");

        // token descriptor
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email ?? ""),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString())
            }),
            Expires = DateTime.Now.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var securityToken = jwtTokenHandler.CreateToken(tokenDescriptor);
        var token = jwtTokenHandler.WriteToken(securityToken);

        return token;
    }
}
