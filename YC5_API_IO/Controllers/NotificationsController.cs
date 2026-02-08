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
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationInterface _notificationService;

        public NotificationsController(INotificationInterface notificationService)
        {
            _notificationService = notificationService;
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User ID not found.");
        }

        // GET: api/Notifications
        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            try
            {
                var userId = GetUserId();
                var notifications = await _notificationService.GetNotificationsAsync(userId);
                return Ok(new
                {
                    success = true,
                    message = "Notifications retrieved successfully",
                    data = notifications.Select(n => new NotificationDto
                    {
                        NotificationId = n.NotificationId,
                        UserId = n.UserId,
                        Message = n.Message,
                        IsRead = n.IsRead,
                        CreatedAt = n.CreatedAt,
                        RelatedEntityType = n.RelatedEntityType,
                        RelatedEntityId = n.RelatedEntityId
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

        // GET: api/Notifications/{notificationId}
        [HttpGet("{notificationId}")]
        public async Task<IActionResult> GetNotification(string notificationId)
        {
            try
            {
                var userId = GetUserId();
                var notification = await _notificationService.GetNotificationByIdAsync(userId, notificationId);

                if (notification == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Notification not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Notification retrieved successfully",
                    data = new NotificationDto
                    {
                        NotificationId = notification.NotificationId,
                        UserId = notification.UserId,
                        Message = notification.Message,
                        IsRead = notification.IsRead,
                        CreatedAt = notification.CreatedAt,
                        RelatedEntityType = notification.RelatedEntityType,
                        RelatedEntityId = notification.RelatedEntityId
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

        // POST: api/Notifications
        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDto createNotificationDto)
        {
            try
            {
                var userId = GetUserId();
                var newNotification = await _notificationService.CreateNotificationAsync(userId, createNotificationDto);

                return CreatedAtAction(nameof(GetNotification), new { notificationId = newNotification.NotificationId }, new
                {
                    success = true,
                    message = "Notification created successfully",
                    data = new NotificationDto
                    {
                        NotificationId = newNotification.NotificationId,
                        UserId = newNotification.UserId,
                        Message = newNotification.Message,
                        IsRead = newNotification.IsRead,
                        CreatedAt = newNotification.CreatedAt,
                        RelatedEntityType = newNotification.RelatedEntityType,
                        RelatedEntityId = newNotification.RelatedEntityId // Corrected from NotificationId to RelatedEntityId
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

        // PUT: api/Notifications/{notificationId}
        [HttpPut("{notificationId}")]
        public async Task<IActionResult> UpdateNotification(string notificationId, [FromBody] UpdateNotificationDto updateNotificationDto)
        {
            try
            {
                var userId = GetUserId();
                var updatedNotification = await _notificationService.UpdateNotificationAsync(userId, notificationId, updateNotificationDto);

                if (updatedNotification == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Notification not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Notification updated successfully",
                    data = new NotificationDto
                    {
                        NotificationId = updatedNotification.NotificationId,
                        UserId = updatedNotification.UserId,
                        Message = updatedNotification.Message,
                        IsRead = updatedNotification.IsRead,
                        CreatedAt = updatedNotification.CreatedAt,
                        RelatedEntityType = updatedNotification.RelatedEntityType,
                        RelatedEntityId = updatedNotification.RelatedEntityId
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

        // DELETE: api/Notifications/{notificationId}
        [HttpDelete("{notificationId}")]
        public async Task<IActionResult> DeleteNotification(string notificationId)
        {
            try
            {
                var userId = GetUserId();
                var result = await _notificationService.DeleteNotificationAsync(userId, notificationId);

                if (!result)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Notification not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Notification deleted successfully"
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

        // PUT: api/Notifications/{notificationId}/mark-as-read
        [HttpPut("{notificationId}/mark-as-read")]
        public async Task<IActionResult> MarkNotificationAsRead(string notificationId)
        {
            try
            {
                var userId = GetUserId();
                var result = await _notificationService.MarkNotificationAsReadAsync(userId, notificationId);

                if (!result)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Notification not found"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Notification marked as read successfully"
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
