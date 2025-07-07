using GeoJourneyer.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using GeoJourneyer.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GeoJouneyer.Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _service;
    private readonly ITokenService _tokenService;

    public AuthController(IUserService service, ITokenService tokenService)
    {
        _service = service;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public IActionResult Register([FromForm] RegisterUserDto dto)
    {
        var token = _service.Register(dto);
        if (token == null) return Conflict();
        return Ok(new { token });
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginUserDto dto)
    {
        var token = _service.Authenticate(dto);
        if (token == null) return Unauthorized();
        return Ok(new { token });
    }

    [Authorize]
    [HttpPost("refresh")]
    public IActionResult Refresh()
    {
        var sub = User.FindFirst(JwtRegisteredClaimNames.Sub) ?? User.FindFirst(ClaimTypes.NameIdentifier);
        if (sub == null) return Unauthorized();
        var token = _tokenService.CreateToken(int.Parse(sub.Value));
        return Ok(new { token });
    }
}
