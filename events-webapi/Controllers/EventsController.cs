using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using events_webapi.Models;
using events_webapi.Services;

namespace events_webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventApiService _service;

        public EventsController(IEventApiService service)
        {
            _service = service;
        }

        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvent()
        {
            var events = await _service.GetAllAsync();
            return Ok(events);
        }

        // GET: api/Events/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var e = await _service.GetByIdAsync(id);
            if (e == null) return NotFound();
            return Ok(e);
        }

        // GET: api/Events/filter
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Event>>> Filter(
            [FromQuery] string? naziv,
            [FromQuery] string? lokacija,
            [FromQuery] DateTime? datumOd,
            [FromQuery] DateTime? datumDo,
            [FromQuery] int? vrstaId,
            [FromQuery] bool? aktivan)
        {
            var result = await _service.FilterAsync(
                naziv,
                lokacija,
                datumOd,
                datumDo,
                vrstaId,
                aktivan);

            return Ok(result);
        }

        // PUT: api/Events/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, Event @event)
        {
            var ok = await _service.UpdateAsync(id, @event);
            if (!ok) return NotFound();
            return NoContent();
        }

        // POST: api/Events
        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent(Event @event)
        {
            var created = await _service.CreateAsync(@event);
            return CreatedAtAction(nameof(GetEvent), new { id = created.Id }, created);
        }

        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}