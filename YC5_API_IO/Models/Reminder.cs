using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YC5_API_IO.Models
{
    public class Reminder
    {
        [Key]
        [Required]
        [StringLength(50)]
        public string ReminderId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(50)]
        public string UserId { get; set; } = string.Empty; // User who owns the reminder

        [Required]
        [StringLength(50)]
        public string TaskId { get; set; } = string.Empty; // Task associated with the reminder

        [Required]
        [StringLength(250)]
        public string ReminderMessage { get; set; } = string.Empty;

        [Required]
        public DateTime ReminderTime { get; set; } = DateTime.UtcNow;

        public bool IsTriggered { get; set; } = false; // Indicates if the reminder has been triggered

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("UserId")]
        public User? User { get; set; }

        [ForeignKey("TaskId")]
        public Task? Task { get; set; }
    }
}
