using System;

namespace YC5_API_IO.Dto
{
    public class ReminderDto
    {
        public string ReminderId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string TaskId { get; set; } = string.Empty;
        public string ReminderMessage { get; set; } = string.Empty;
        public DateTime ReminderTime { get; set; }
        public bool IsTriggered { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
