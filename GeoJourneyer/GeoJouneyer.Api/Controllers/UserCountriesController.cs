using GeoJourneyer.Application.Services.Interfaces;
using GeoJourneyer.Domain;
using Microsoft.AspNetCore.Mvc;

namespace GeoJourneyer.Api.Controllers;

[ApiController]
[Route("api/user-countries")]
public class UserCountriesController : ControllerBase
{
    private readonly IUserCountryService _service;

    public UserCountriesController(IUserCountryService service)
    {
        _service = service;
    }

    [HttpGet("{userId}")]
    public IActionResult Get(int userId)
    {
        return Ok(_service.GetUserCountries(userId));
    }

    [HttpPost]
    public IActionResult Post([FromBody] UserCountry country)
    {
        var id = _service.AddUserCountry(country);
        return Ok(id);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var success = _service.DeleteUserCountry(id);
        if (!success) return Conflict();
        return NoContent();
    }
}