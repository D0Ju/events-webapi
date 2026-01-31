using events_webapi.Models;

namespace events_webapi.Services
{
    public interface IEventTypeApiService
    {
        Task<IEnumerable<EventType>> GetAllAsync();
        Task<EventType?> GetByIdAsync(int id);
        Task<EventType> CreateAsync(EventType e);
        Task<bool> UpdateAsync(int id, EventType e);
        Task<bool> DeleteAsync(int id);
    }
}
