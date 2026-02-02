using events_webapi.Models;

namespace events_webapi.Services
{
    public interface IEventApiService
    {
        Task<IEnumerable<Event>> GetAllAsync();
        Task<IEnumerable<Event>> GetAllByUserAsync(int userId); 
        Task<Event?> GetByIdAsync(int id);
        Task<Event> CreateAsync(Event e);
        Task<bool> UpdateAsync(int id, Event e);
        Task<bool> DeleteAsync(int id);

        // Filter method
        Task<IEnumerable<Event>> FilterAsync(
            int userId,
            string? naziv,
            string? lokacija,
            DateTime? datumOd,
            DateTime? datumDo,
            int? vrstaId,
            bool? aktivan);
    }
}
