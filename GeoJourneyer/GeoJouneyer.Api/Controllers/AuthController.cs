using GeoJourneyer.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GeoJouneyer.Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _service;

    public AuthController(IUserService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] UserDto dto)
    {
        var token = _service.Register(dto.Username, dto.Password);
        return Ok(token);
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] UserDto dto)
    {
        var token = _service.Authenticate(dto.Username, dto.Password);
        if (token == null) return Unauthorized();
        return Ok(token);
    }

    public record UserDto(string Username, string Password);
}
