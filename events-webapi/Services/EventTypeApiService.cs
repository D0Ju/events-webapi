using events_webapi.Models;
using Microsoft.EntityFrameworkCore;

namespace events_webapi.Services
{
    public class EventTypeApiService : IEventTypeApiService
    {
        private readonly AppdbContext _context;

        public EventTypeApiService(AppdbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EventType>> GetAllAsync()
        {
            return await _context.EventType.ToListAsync();
        }

        public async Task<EventType?> GetByIdAsync(int id)
        {
            return await _context.EventType.FindAsync(id);
        }

        public async Task<EventType> CreateAsync(EventType e)
        {
            _context.EventType.Add(e);
            await _context.SaveChangesAsync();
            return e;
        }

        public async Task<bool> UpdateAsync(int id, EventType e)
        {
            if (id != e.Id) return false;

            _context.Entry(e).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.EventType.AnyAsync(x => x.Id == id))
                    return false;
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var e = await _context.EventType.FindAsync(id);
            if (e == null) return false;

            _context.EventType.Remove(e);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
