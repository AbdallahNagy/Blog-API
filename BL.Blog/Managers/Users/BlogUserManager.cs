using Blog.BL.DTOs.Users;
using Blog.BL.Exception_Handling;
using Blog.DAL.Models;
using Blog.DAL.Repos.RefreshTokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Blog.BL.Managers.Users;

public class BlogUserManager : IUserManager
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IRefreshTokenRepo _refreshTokenRepo;
    private readonly TokenValidationParameters _tokenValidationParameters;

    public BlogUserManager(UserManager<User> userManager, 
        IConfiguration configuration, 
        IRefreshTokenRepo refreshTokenRepo, 
        TokenValidationParameters tokenValidationParameters)
    {
        _userManager = userManager;
        _configuration = configuration;
        _refreshTokenRepo = refreshTokenRepo;
        _tokenValidationParameters = tokenValidationParameters;
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
            errorsSerialized.Append($"[{i + 1}] {errors[i]} ");
        }

        if (!isUserCreated.Succeeded) throw new BusinessException(500, "Internal Server Error: Couldn't create user.", errorsSerialized.ToString());

        // generate user token
        var tokenResponse = GenerateToken(newUser);

        return await tokenResponse;
    }

    public async Task<TokenRespnseDTO> Login(LoginDTO userData)
    {
        var existingUser = await _userManager.FindByEmailAsync(userData.Email)
            ?? throw new BusinessException(404, "Invalid Email or Passwod");

        var isCorrect = await _userManager.CheckPasswordAsync(existingUser, userData.Password);

        if (!isCorrect) throw new BusinessException(404, "Invalid Email or Passwod");

        var tokenResponse = GenerateToken(existingUser);

        return await tokenResponse;
    }

    public TokenRespnseDTO Tokens(TokenRequestDTO tokenRequest)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        try
        {
            _tokenValidationParameters.ValidateLifetime = false;

            var tokenVerification = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParameters, out var validatedToken);

            if(validatedToken is JwtSecurityToken jwtSecurityToken)
            {
                var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256);
            }
        }
        catch (Exception ex)
        {
            throw;
        }

        return new TokenRespnseDTO("","");
    }

    private async Task<TokenRespnseDTO> GenerateToken(User user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JWTConfig:SecretKey").Value ?? "ld2e5nvi1adkq");

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
            Expires = DateTime.UtcNow.Add(TimeSpan.Parse(_configuration.GetSection("JWTConfig:ExpiryTimeFrame").Value ?? "00:01:00")),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var securityToken = jwtTokenHandler.CreateToken(tokenDescriptor);
        var token = jwtTokenHandler.WriteToken(securityToken);

        var refreshToken = new RefreshToken
        {
            Token = RandonStringGeneration(23),
            JwtId = securityToken.Id,
            CreatedAt = DateTime.UtcNow,
            ExpiresOn = DateTime.UtcNow.AddMonths(6),
            UserId = user.Id
        };

        await _refreshTokenRepo.Add(refreshToken);
        await _refreshTokenRepo.SaveChanges();

        return new TokenRespnseDTO(token, refreshToken.Token);
    }

    private static string RandonStringGeneration(int length)
    {
        var random = new Random();
        var chars = "_ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890qwretaysduifogplkjhnvmnzxc";
        return new string(Enumerable
            .Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)])
            .ToArray());
    }
}
