using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YC5_API_IO.Models
{
    public class Comment
    {
        [Key]
        [Required]
        [StringLength(50)]
        public string CommentId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(50)]
        public string TaskId { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string UserId { get; set; } = string.Empty; // User who made the comment

        [Required]
        [StringLength(50)]
        public string CommentTitle { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string CommentText { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property for related Attachments
        public ICollection<Attachment>? Attachments { get; set; }

        // Navigation property for User
        [ForeignKey("UserId")]
        public User? User { get; set; }

        // Navigation property for Task
        [ForeignKey("TaskId")]
        public Task? Task { get; set; }
    }
}
