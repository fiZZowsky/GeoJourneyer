using GeoJourneyer.Application.Services.Interfaces;
using GeoJourneyer.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GeoJourneyer.Api.Controllers;

[ApiController]
[Route("api/countries")]
public class CountriesController : ControllerBase
{
    private readonly ICountryService _service;

    public CountriesController(ICountryService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_service.GetAll());
    }

    [HttpPost]
    public IActionResult Post([FromBody] Country country)
    {
        var id = _service.AddCountry(country);
        return Ok(id);
    }
}
