using events_webapi.Models;
using Microsoft.EntityFrameworkCore;
using events_webapi.Data;
namespace events_webapi.Services
{
    public class EventApiService : IEventApiService
    {
        private readonly AppdbContext _context;

        public EventApiService(AppdbContext context)
        {
            _context = context;
        }        

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await _context.Event
                .Include(e => e.Vrsta)
                .ToListAsync();
        }

        public async Task<IEnumerable<Event>> GetAllByUserAsync(int userId){
            return await _context.Event
                .Where(e => e.UserId == userId)
                .Include(e => e.Vrsta)
                .ToListAsync();
        }

        public async Task<Event?> GetByIdAsync(int id)
        {
            return await _context.Event
                .Include(e => e.Vrsta)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Event> CreateAsync(Event e)
        {
            _context.Event.Add(e);
            await _context.SaveChangesAsync();
            return e;
        }
        /*
        public async Task<bool> UpdateAsync(int id, Event e)
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
                if (!await _context.Event.AnyAsync(x => x.Id == id))
                    return false;
                throw;
            }
        }
        */
        public async Task<bool> UpdateAsync(int id, Event e){
            if (id != e.Id) return false;

            try
            {
                // Get the existing event from DB first
                var existingEvent = await _context.Event.FindAsync(id);
                
                if (existingEvent == null)
                    return false;

                // Update only the fields
                existingEvent.Naziv = e.Naziv;
                existingEvent.Lokacija = e.Lokacija;
                existingEvent.DatumPocetka = e.DatumPocetka;
                existingEvent.DatumZavrsetka = e.DatumZavrsetka;
                existingEvent.BrojPolaznika = e.BrojPolaznika;
                existingEvent.Cijena = e.Cijena;
                existingEvent.Opis = e.Opis;
                existingEvent.VrstaId = e.VrstaId;
                existingEvent.Aktivan = e.Aktivan;
                // UserId stays the same

                _context.Event.Update(existingEvent);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Event.AnyAsync(x => x.Id == id))
                    return false;
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var e = await _context.Event.FindAsync(id);
            if (e == null) return false;

            _context.Event.Remove(e);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Event>> FilterAsync(
            string? naziv,
            string? lokacija,
            DateTime? datumOd,
            DateTime? datumDo,
            int? vrstaId,
            bool? aktivan)
        {
            var query = _context.Event
                .Include(e => e.Vrsta)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(naziv))
                query = query.Where(e => e.Naziv.Contains(naziv));

            if (!string.IsNullOrWhiteSpace(lokacija))
                query = query.Where(e => e.Lokacija.Contains(lokacija));

            if (datumOd.HasValue)
                query = query.Where(e => e.DatumPocetka >= datumOd);

            if (datumDo.HasValue)
                query = query.Where(e => e.DatumZavrsetka <= datumDo);

            if (vrstaId.HasValue)
                query = query.Where(e => e.VrstaId == vrstaId);

            if (aktivan.HasValue)
                query = query.Where(e => e.Aktivan == aktivan);

            return await query.ToListAsync();
        }
    }
}
