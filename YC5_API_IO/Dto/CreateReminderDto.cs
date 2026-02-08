using System;
using System.ComponentModel.DataAnnotations;

namespace YC5_API_IO.Dto
{
    public class CreateReminderDto
    {
        [Required(ErrorMessage = "Task ID is required.")]
        public string TaskId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Reminder Message is required.")]
        [StringLength(250, ErrorMessage = "Reminder Message cannot exceed 250 characters.")]
        public string ReminderMessage { get; set; } = string.Empty;

        [Required(ErrorMessage = "Reminder Time is required.")]
        public DateTime ReminderTime { get; set; } = DateTime.UtcNow;
    }
}
