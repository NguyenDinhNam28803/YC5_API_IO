using YC5_API_IO.Dto;
using YC5_API_IO.Models;

namespace YC5_API_IO.Interfaces
{
    public interface IReminderInterface
    {
        Task<IEnumerable<Reminder>> GetRemindersAsync(string userId);
        Task<Reminder?> GetReminderByIdAsync(string userId, string reminderId);
        Task<Reminder> CreateReminderAsync(string userId, CreateReminderDto createReminderDto);
        Task<Reminder?> UpdateReminderAsync(string userId, string reminderId, UpdateReminderDto updateReminderDto);
        Task<bool> DeleteReminderAsync(string userId, string reminderId);
    }
}
