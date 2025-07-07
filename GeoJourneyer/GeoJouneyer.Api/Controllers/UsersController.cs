using GeoJourneyer.Application.DTOs;
using GeoJourneyer.Application.Services.Interfaces;
using GeoJourneyer.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GeoJouneyer.Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserCountryService _countryService;
    private readonly ICountryService _countries;

    public UsersController(IUserService userService, IUserCountryService countryService, ICountryService countries)
    {
        _userService = userService;
        _countryService = countryService;
        _countries = countries;
    }

    [HttpGet("me")]
    public IActionResult GetProfile()
    {
        var sub = User.FindFirst(JwtRegisteredClaimNames.Sub) ?? User.FindFirst(ClaimTypes.NameIdentifier);
        if (sub == null) return Unauthorized();
        var id = int.Parse(sub.Value);
        var user = _userService.GetById(id);
        if (user == null) return NotFound();

        var visited = _countryService.GetUserCountries(id)
            .Count(c => c.Status == CountryStatus.Visited || c.Status == CountryStatus.Living);
        var total = _countries.GetAll().Count();

        var dto = new UserProfileDTO
        {
            Username = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Age = user.Age,
            CountryOfOrigin = user.CountryOfOrigin,
            PhotoUrl = user.Photo != null ? $"data:image/jpeg;base64,{Convert.ToBase64String(user.PhotoUrl)}" : null,
            VisitedCount = visited,
            VisitedPercent = total > 0 ? visited * 100.0 / total : 0
        };

        return Ok(dto);
    }
}