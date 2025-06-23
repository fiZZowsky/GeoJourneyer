using GeoJourneyer.Application.Services.Interfaces;
using GeoJourneyer.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace GeoJourneyer.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TravelPlansController : ControllerBase
{
    private readonly ITravelPlanService _service;

    public TravelPlansController(ITravelPlanService service)
    {
        _service = service;
    }

    [HttpGet("{userId}")]
    public IActionResult GetPlans(int userId)
    {
        return Ok(_service.GetPlans(userId));
    }

    [HttpPost]
    public IActionResult Create([FromBody] TravelPlanDto dto)
    {
        if (dto.PlaceIds == null || !dto.PlaceIds.Any())
        {
            return BadRequest("No places specified");
        }

        var id = _service.CreatePlan(
            new TravelPlan { UserId = dto.UserId, CountryId = dto.CountryId, Name = dto.Name },
            dto.PlaceIds);
        return Ok(id);
    }

    [HttpGet("{planId}/stops")]
    public IActionResult GetStops(int planId)
    {
        return Ok(_service.GetPlanStops(planId));
    }

    [HttpPost("{planId}/optimize")]
    public IActionResult Optimize(int planId)
    {
        var stops = _service.GetPlanStops(planId);
        var places = _service.GetPlaces(stops.Select(s => s.PlaceId));
        var optimized = _service.OptimizeRoute(places);
        _service.SaveStops(planId, optimized);
        return Ok(optimized);
    }

    public record TravelPlanDto(int UserId, int CountryId, string Name, IEnumerable<int> PlaceIds);
}