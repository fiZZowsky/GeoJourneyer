using GeoJourneyer.Application.Services.Interfaces;
using GeoJourneyer.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeoJouneyer.Api.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _service;

    public NotificationsController(INotificationService service)
    {
        _service = service;
    }

    [HttpGet("{userId}")]
    public IActionResult Get(int userId)
    {
        return Ok(_service.GetForUser(userId));
    }

    [HttpPost]
    public IActionResult Post([FromBody] Notification notification)
    {
        var id = _service.Add(notification);
        return Ok(id);
    }

    [HttpPost("{id}/read")]
    public IActionResult MarkAsRead(int id)
    {
        _service.MarkAsRead(id);
        return NoContent();
    }
}