using YC5_API_IO.Dto;
using YC5_API_IO.Models;

namespace YC5_API_IO.Interfaces
{
    public interface INotificationInterface
    {
        Task<IEnumerable<Notification>> GetNotificationsAsync(string userId);
        Task<Notification?> GetNotificationByIdAsync(string userId, string notificationId);
        Task<Notification> CreateNotificationAsync(string userId, CreateNotificationDto createNotificationDto);
        Task<Notification?> UpdateNotificationAsync(string userId, string notificationId, UpdateNotificationDto updateNotificationDto);
        Task<bool> DeleteNotificationAsync(string userId, string notificationId);
        Task<bool> MarkNotificationAsReadAsync(string userId, string notificationId);
    }
}
