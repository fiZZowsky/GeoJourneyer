using GeoJourneyer.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GeoJouneyer.Api.Controllers;

[Route("api/[controller]")]
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
        var id = _service.Register(dto.Username, dto.Password);
        return Ok(id);
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] UserDto dto)
    {
        var id = _service.Authenticate(dto.Username, dto.Password);
        if (id == null) return Unauthorized();
        return Ok(id.Value);
    }

    public record UserDto(string Username, string Password);
}
