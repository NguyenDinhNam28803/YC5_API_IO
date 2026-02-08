using Microsoft.EntityFrameworkCore;
using YC5_API_IO.Data;
using YC5_API_IO.Dto;
using YC5_API_IO.Interfaces;
using YC5_API_IO.Models;

namespace YC5_API_IO.Services
{
    public class ReminderService : IReminderInterface
    {
        private readonly ApplicationDbContext _context;

        public ReminderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reminder>> GetRemindersAsync(string userId)
        {
            return await _context.Reminders
                                 .Where(r => r.UserId == userId)
                                 .ToListAsync();
        }

        public async Task<Reminder?> GetReminderByIdAsync(string userId, string reminderId)
        {
            return await _context.Reminders
                                 .Where(r => r.UserId == userId && r.ReminderId == reminderId)
                                 .FirstOrDefaultAsync();
        }

        public async Task<Reminder> CreateReminderAsync(string userId, CreateReminderDto createReminderDto)
        {
            var reminder = new Reminder
            {
                UserId = userId,
                TaskId = createReminderDto.TaskId,
                ReminderMessage = createReminderDto.ReminderMessage,
                ReminderTime = createReminderDto.ReminderTime,
                IsTriggered = false, // Default status
                CreatedAt = DateTime.UtcNow
            };

            _context.Reminders.Add(reminder);
            await _context.SaveChangesAsync();
            return reminder;
        }

        public async Task<Reminder?> UpdateReminderAsync(string userId, string reminderId, UpdateReminderDto updateReminderDto)
        {
            var reminder = await _context.Reminders
                                         .Where(r => r.UserId == userId && r.ReminderId == reminderId)
                                         .FirstOrDefaultAsync();

            if (reminder == null)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(updateReminderDto.ReminderMessage))
            {
                reminder.ReminderMessage = updateReminderDto.ReminderMessage;
            }
            if (updateReminderDto.ReminderTime.HasValue)
            {
                reminder.ReminderTime = updateReminderDto.ReminderTime.Value;
            }
            if (updateReminderDto.IsTriggered.HasValue)
            {
                reminder.IsTriggered = updateReminderDto.IsTriggered.Value;
            }
            
            _context.Reminders.Update(reminder);
            await _context.SaveChangesAsync();
            return reminder;
        }

        public async Task<bool> DeleteReminderAsync(string userId, string reminderId)
        {
            var reminder = await _context.Reminders
                                         .Where(r => r.UserId == userId && r.ReminderId == reminderId)
                                         .FirstOrDefaultAsync();

            if (reminder == null)
            {
                return false;
            }

            _context.Reminders.Remove(reminder);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
