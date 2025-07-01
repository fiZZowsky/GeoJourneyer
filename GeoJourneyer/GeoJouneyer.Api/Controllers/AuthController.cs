using GeoJourneyer.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using GeoJourneyer.Application.DTOs;

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
    public IActionResult Register([FromBody] RegisterUserDto dto)
    {
        var token = _service.Register(dto);
        if (token == null) return Conflict();
        return Ok(token);
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginUserDto dto)
    {
        var token = _service.Authenticate(dto);
        if (token == null) return Unauthorized();
        return Ok(token);
    }
}
