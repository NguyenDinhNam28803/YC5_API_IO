using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YC5_API_IO.Dto;
using YC5_API_IO.Interfaces;
using YC5_API_IO.Models;

namespace YC5_API_IO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RemindersController : ControllerBase
    {
        private readonly IReminderInterface _reminderService;

        public RemindersController(IReminderInterface reminderService)
        {
            _reminderService = reminderService;
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User ID not found.");
        }

        // GET: api/Reminders
        [HttpGet]
        public async Task<IActionResult> GetReminders()
        {
            try
            {
                var userId = GetUserId();
                var reminders = await _reminderService.GetRemindersAsync(userId);
                return Ok(new
                {
                    success = true,
                    message = "Reminders retrieved successfully",
                    data = reminders.Select(r => new ReminderDto
                    {
                        ReminderId = r.ReminderId,
                        UserId = r.UserId,
                        TaskId = r.TaskId,
                        ReminderMessage = r.ReminderMessage,
                        ReminderTime = r.ReminderTime,
                        IsTriggered = r.IsTriggered,
                        CreatedAt = r.CreatedAt
                    })
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // GET: api/Reminders/{reminderId}
        [HttpGet("{reminderId}")]
        public async Task<IActionResult> GetReminder(string reminderId)
        {
            try
            {
                var userId = GetUserId();
                var reminder = await _reminderService.GetReminderByIdAsync(userId, reminderId);

                if (reminder == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Reminder not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Reminder retrieved successfully",
                    data = new ReminderDto
                    {
                        ReminderId = reminder.ReminderId,
                        UserId = reminder.UserId,
                        TaskId = reminder.TaskId,
                        ReminderMessage = reminder.ReminderMessage,
                        ReminderTime = reminder.ReminderTime,
                        IsTriggered = reminder.IsTriggered,
                        CreatedAt = reminder.CreatedAt
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // POST: api/Reminders
        [HttpPost]
        public async Task<IActionResult> CreateReminder([FromBody] CreateReminderDto createReminderDto)
        {
            try
            {
                var userId = GetUserId();
                var newReminder = await _reminderService.CreateReminderAsync(userId, createReminderDto);

                return CreatedAtAction(nameof(GetReminder), new { reminderId = newReminder.ReminderId }, new
                {
                    success = true,
                    message = "Reminder created successfully",
                    data = new ReminderDto
                    {
                        ReminderId = newReminder.ReminderId,
                        UserId = newReminder.UserId,
                        TaskId = newReminder.TaskId,
                        ReminderMessage = newReminder.ReminderMessage,
                        ReminderTime = newReminder.ReminderTime,
                        IsTriggered = newReminder.IsTriggered,
                        CreatedAt = newReminder.CreatedAt
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // PUT: api/Reminders/{reminderId}
        [HttpPut("{reminderId}")]
        public async Task<IActionResult> UpdateReminder(string reminderId, [FromBody] UpdateReminderDto updateReminderDto)
        {
            try
            {
                var userId = GetUserId();
                var updatedReminder = await _reminderService.UpdateReminderAsync(userId, reminderId, updateReminderDto);

                if (updatedReminder == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Reminder not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Reminder updated successfully",
                    data = new ReminderDto
                    {
                        ReminderId = updatedReminder.ReminderId,
                        UserId = updatedReminder.UserId,
                        TaskId = updatedReminder.TaskId,
                        ReminderMessage = updatedReminder.ReminderMessage,
                        ReminderTime = updatedReminder.ReminderTime,
                        IsTriggered = updatedReminder.IsTriggered,
                        CreatedAt = updatedReminder.CreatedAt
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // DELETE: api/Reminders/{reminderId}
        [HttpDelete("{reminderId}")]
        public async Task<IActionResult> DeleteReminder(string reminderId)
        {
            try
            {
                var userId = GetUserId();
                var result = await _reminderService.DeleteReminderAsync(userId, reminderId);

                if (!result)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Reminder not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Reminder deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}
