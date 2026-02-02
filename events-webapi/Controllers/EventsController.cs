using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using events_webapi.Services;
using events_webapi.Models;
using System.Security.Claims;

namespace events_webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] 
public class EventsController : ControllerBase
{
    private readonly IEventApiService _service;

    public EventsController(IEventApiService service)
    {
        _service = service;
    }

    private int GetUserId()
    {
        return int.Parse(User.FindFirst("userId")?.Value ?? "0");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Event>>> GetAllEvents()
    {
        var userId = GetUserId();
        var events = await _service.GetAllByUserAsync(userId);
        return Ok(events);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Event>> GetEvent(int id)
    {
        var userId = GetUserId();
        var ev = await _service.GetByIdAsync(id);
        
        if (ev == null || ev.UserId != userId)
            return NotFound();
            
        return Ok(ev);
    }
    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcoming()
    {
        var userId = GetUserId();
        var upcoming = await _service.GetAllByUserAsync(userId);
        
        // Filter for future events
        var futureEvents = upcoming
            .Where(e => e.DatumPocetka > DateTime.UtcNow)
            .OrderBy(e => e.DatumPocetka)
            .ToList();
        
        return Ok(futureEvents);
    }
    [HttpPost]
    public async Task<ActionResult<Event>> CreateEvent(Event @event)
    {
        @event.UserId = GetUserId(); 
        var result = await _service.CreateAsync(@event);
        return CreatedAtAction(nameof(GetEvent), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(int id, Event @event)
    {
        var userId = GetUserId();
        var existingEvent = await _service.GetByIdAsync(id);
        
        if (existingEvent == null || existingEvent.UserId != userId)
            return NotFound();

        @event.UserId = userId; 
        var success = await _service.UpdateAsync(id, @event);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var userId = GetUserId();
        var ev = await _service.GetByIdAsync(id);
        
        if (ev == null || ev.UserId != userId)
            return NotFound();

        var success = await _service.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }

    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<Event>>> FilterEvents(
        [FromQuery] string? naziv,
        [FromQuery] string? lokacija,
        [FromQuery] DateTime? datumOd,
        [FromQuery] DateTime? datumDo,
        [FromQuery] int? vrstaId,
        [FromQuery] bool? aktivan)
    {
        var userId = GetUserId();
        var events = await _service.FilterAsync(userId, naziv, lokacija, datumOd, datumDo, vrstaId, aktivan);
        return Ok(events);
    }
}
