using Blog.BL.DTOs.Users;
using Blog.BL.Managers.Users;
using Blog.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

[Route("api/v1/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserManager _userManager;

    public UserController(IUserManager userManager)
    {
        _userManager = userManager;
    }

    [HttpPost]
    [Route("register")]
    public async Task<ActionResult> Post([FromBody] RegistrationDTO userData)
    {
        if (!ModelState.IsValid) return BadRequest();

        var response = await _userManager.Registration(userData);
        return Ok(response);
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult> Post([FromBody] LoginDTO userData)
    {
        if (!ModelState.IsValid) return BadRequest();

        var response = await _userManager.Login(userData);
        return Ok(response);
    }

    [HttpPost]
    [Route("tokens")]
    public async Task<ActionResult> Tokens([FromBody] TokenRequestDTO tokenRequest)
    {
        if (!ModelState.IsValid) return BadRequest();

        var response = await _userManager.VerifyGenerateTokens(tokenRequest);
        return Ok(response);
    }
}
