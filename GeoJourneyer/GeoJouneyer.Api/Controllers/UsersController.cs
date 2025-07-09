using GeoJourneyer.Application.DTOs;
using GeoJourneyer.Application.Services.Interfaces;
using GeoJourneyer.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Linq;

namespace GeoJouneyer.Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserCountryService _countryService;
    private readonly ICountryService _countries;
    private readonly IFriendRequestService _friendService;

    public UsersController(IUserService userService, IUserCountryService countryService, ICountryService countries, IFriendRequestService friendService)
    {
        _userService = userService;
        _countryService = countryService;
        _countries = countries;
        _friendService = friendService;
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
            PhotoUrl = user.Photo != null
                ? $"data:image/jpeg;base64,{Convert.ToBase64String(user.Photo)}"
                : null,
            VisitedCount = visited,
            VisitedPercent = total > 0 ? visited * 100.0 / total : 0
        };

        return Ok(dto);
    }


    [HttpGet("search")]
    public IActionResult Search([FromQuery] string q)
    {
        var users = _userService.SearchByUsername(q);
        return Ok(users.Select(u => new PublicUserDTO(u.Id, u.Username)));
    }

    [HttpGet("{id}/friends")]
    public IActionResult GetFriends(int id)
    {
        var friends = _userService.GetFriends(id)
            .Select(u => new PublicUserDTO(u.Id, u.Username));
        return Ok(friends);
    }

    [HttpGet("{id}")]
    public IActionResult GetPublicProfile(int id)
    {
        var user = _userService.GetById(id);
        if (user == null) return NotFound();

        var visited = _countryService.GetUserCountries(id)
            .Count(c => c.Status == CountryStatus.Visited || c.Status == CountryStatus.Living);
        var total = _countries.GetAll().Count();

        var dto = new PublicUserProfileDTO
        {
            Username = user.Username,
            FirstName = user.FirstName,
            CountryOfOrigin = user.CountryOfOrigin,
            PhotoUrl = user.Photo != null ? $"data:image/jpeg;base64,{Convert.ToBase64String(user.Photo)}" : null,
            VisitedCount = visited,
            VisitedPercent = total > 0 ? visited * 100.0 / total : 0
        };

        return Ok(dto);
    }

    [HttpPost("{id}/invite")]
    public IActionResult Invite(int id)
    {
        var sub = User.FindFirst(JwtRegisteredClaimNames.Sub) ?? User.FindFirst(ClaimTypes.NameIdentifier);
        if (sub == null) return Unauthorized();
        var fromId = int.Parse(sub.Value);
        if (fromId == id) return BadRequest();

        var result = _friendService.SendRequest(fromId, id);
        if (result == 0) return Conflict();
        return Ok(result);
    }

    [HttpGet("{id}/invite")]
    public IActionResult GetInvite(int id)
    {
        var sub = User.FindFirst(JwtRegisteredClaimNames.Sub) ?? User.FindFirst(ClaimTypes.NameIdentifier);
        if (sub == null) return Unauthorized();
        var fromId = int.Parse(sub.Value);
        var req = _friendService.GetBetweenUsers(fromId, id);
        if (req == null) return NotFound();
        return Ok(req);
    }

    [HttpPost("{id}/accept")]
    public IActionResult Accept(int id)
    {
        var sub = User.FindFirst(JwtRegisteredClaimNames.Sub) ?? User.FindFirst(ClaimTypes.NameIdentifier);
        if (sub == null) return Unauthorized();
        var toId = int.Parse(sub.Value);
        var req = _friendService.GetBetweenUsers(id, toId);
        if (req == null) return NotFound();
        _friendService.UpdateStatus(req.Id, FriendRequestStatus.Accepted);
        return Ok();
    }

    [HttpPost("{id}/reject")]
    public IActionResult Reject(int id)
    {
        var sub = User.FindFirst(JwtRegisteredClaimNames.Sub) ?? User.FindFirst(ClaimTypes.NameIdentifier);
        if (sub == null) return Unauthorized();
        var toId = int.Parse(sub.Value);
        var req = _friendService.GetBetweenUsers(id, toId);
        if (req == null) return NotFound();
        _friendService.UpdateStatus(req.Id, FriendRequestStatus.Rejected);
        return Ok();
    }
}