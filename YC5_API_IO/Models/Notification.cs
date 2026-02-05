using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YC5_API_IO.Models
{
    public class Notification
    {
        [Key]
        [Required]
        [StringLength(50)]
        public string NotificationId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(50)]
        public string UserId { get; set; } // Foreign key to User

        [ForeignKey("UserId")]
        public User? User { get; set; } // Navigation property

        [Required]
        [StringLength(500)]
        public string Message { get; set; } = string.Empty;

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Optional: To link notification to a specific entity
        [StringLength(50)]
        public string? RelatedEntityType { get; set; } // e.g., "Task", "Comment"

        [StringLength(50)]
        public string? RelatedEntityId { get; set; }
    }
}
