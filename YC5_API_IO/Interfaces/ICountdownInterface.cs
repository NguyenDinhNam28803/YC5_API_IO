using YC5_API_IO.Dto;
using YC5_API_IO.Models;

namespace YC5_API_IO.Interfaces
{
    public interface ICountdownInterface
    {
        Task<IEnumerable<CountDown>> GetCountdownsAsync(string userId);
        Task<CountDown?> GetCountdownByIdAsync(string userId, string countdownId);
        Task<CountDown> CreateCountdownAsync(string userId, CreateCountdownDto createCountdownDto);
        Task<CountDown?> UpdateCountdownAsync(string userId, string countdownId, UpdateCountdownDto updateCountdownDto);
        Task<bool> DeleteCountdownAsync(string userId, string countdownId);
    }
}
