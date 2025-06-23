using GeoJourneyer.Application.Services.Interfaces;
using GeoJourneyer.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GeoJourneyer.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PlacesController : ControllerBase
{
    private readonly IPlaceService _service;

    public PlacesController(IPlaceService service)
    {
        _service = service;
    }

    [HttpPost]
    public IActionResult Post([FromBody] Place place)
    {
        var id = _service.AddPlace(place);
        return Ok(id);
    }

    [HttpGet("/countries/{countryId}/places")]
    public IActionResult GetByCountry(int countryId)
    {
        return Ok(_service.GetByCountry(countryId));
    }
}
