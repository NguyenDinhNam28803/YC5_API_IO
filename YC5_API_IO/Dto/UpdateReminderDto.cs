using System;
using System.ComponentModel.DataAnnotations;

namespace YC5_API_IO.Dto
{
    public class UpdateReminderDto
    {
        [StringLength(250, ErrorMessage = "Reminder Message cannot exceed 250 characters.")]
        public string? ReminderMessage { get; set; }

        public DateTime? ReminderTime { get; set; }

        public bool? IsTriggered { get; set; }
    }
}
