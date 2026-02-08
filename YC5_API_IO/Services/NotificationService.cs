using Microsoft.EntityFrameworkCore;
using YC5_API_IO.Data;
using YC5_API_IO.Dto;
using YC5_API_IO.Interfaces;
using YC5_API_IO.Models;

namespace YC5_API_IO.Services
{
    public class NotificationService : INotificationInterface
    {
        private readonly ApplicationDbContext _context;

        public NotificationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Notification>> GetNotificationsAsync(string userId)
        {
            return await _context.Notifications
                                 .Where(n => n.UserId == userId)
                                 .OrderByDescending(n => n.CreatedAt)
                                 .ToListAsync();
        }

        public async Task<Notification?> GetNotificationByIdAsync(string userId, string notificationId)
        {
            return await _context.Notifications
                                 .Where(n => n.UserId == userId && n.NotificationId == notificationId)
                                 .FirstOrDefaultAsync();
        }

        public async Task<Notification> CreateNotificationAsync(string userId, CreateNotificationDto createNotificationDto)
        {
            var notification = new Notification
            {
                UserId = userId,
                Message = createNotificationDto.Message,
                IsRead = false, // Default status
                CreatedAt = DateTime.UtcNow,
                RelatedEntityType = createNotificationDto.RelatedEntityType,
                RelatedEntityId = createNotificationDto.RelatedEntityId
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<Notification?> UpdateNotificationAsync(string userId, string notificationId, UpdateNotificationDto updateNotificationDto)
        {
            var notification = await _context.Notifications
                                         .Where(n => n.UserId == userId && n.NotificationId == notificationId)
                                         .FirstOrDefaultAsync();

            if (notification == null)
            {
                return null;
            }

            if (updateNotificationDto.IsRead.HasValue)
            {
                notification.IsRead = updateNotificationDto.IsRead.Value;
            }
            
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<bool> DeleteNotificationAsync(string userId, string notificationId)
        {
            var notification = await _context.Notifications
                                         .Where(n => n.UserId == userId && n.NotificationId == notificationId)
                                         .FirstOrDefaultAsync();

            if (notification == null)
            {
                return false;
            }

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkNotificationAsReadAsync(string userId, string notificationId)
        {
            var notification = await _context.Notifications
                                         .Where(n => n.UserId == userId && n.NotificationId == notificationId)
                                         .FirstOrDefaultAsync();

            if (notification == null)
            {
                return false;
            }

            notification.IsRead = true;
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
