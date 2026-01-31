using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using events_webapi.Models;
using events_webapi.Services;
using Microsoft.AspNetCore.Authorization;

namespace events_webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EventTypesController : ControllerBase
    {
        private readonly IEventTypeApiService _service;

        public EventTypesController(IEventTypeApiService service)
        {
            _service = service;
        }

        // GET: api/EventTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventType>>> GetEventType()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        // GET: api/EventTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventType>> GetEventType(int id)
        {
            var et = await _service.GetByIdAsync(id);
            if (et == null) return NotFound();
            return Ok(et);
        }

        // PUT: api/EventTypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEventType(int id, EventType eventType)
        {
            var ok = await _service.UpdateAsync(id, eventType);
            if (!ok) return NotFound();
            return NoContent();
        }

        // POST: api/EventTypes
        [HttpPost]
        public async Task<ActionResult<EventType>> PostEventType(EventType eventType)
        {
            var created = await _service.CreateAsync(eventType);
            return CreatedAtAction(nameof(GetEventType), new { id = created.Id }, created);
        }

        // DELETE: api/EventTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEventType(int id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}