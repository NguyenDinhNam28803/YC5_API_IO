using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YC5_API_IO.Models
{
    public class Attachment
    {
        [Key]
        [Required]
        public string AttachmentId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(255)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string FilePath { get; set; } = string.Empty; // URL or path to storage

        public long FileSize { get; set; } // Size in bytes

        [StringLength(100)]
        public string? MimeType { get; set; } // e.g., "image/jpeg", "application/pdf"

        [Required]
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(50)]
        public string UploadedByUserId { get; set; } // Foreign key to User

        [ForeignKey("UploadedByUserId")]
        public User? UploadedByUser { get; set; } // Navigation property

        // Generic relationship for attachments
        [StringLength(50)]
        public string? RelatedEntityType { get; set; } // e.g., "Task", "Comment"

        [StringLength(50)]
        public string? RelatedEntityId { get; set; }

        // Optional: Specific foreign keys if attachments are primarily for specific entities
        [StringLength(50)]
        public string? TaskId { get; set; }
        [ForeignKey("TaskId")]
        public YC5_API_IO.Models.Task? Task { get; set; }

        [StringLength(50)]
        public string? CommentId { get; set; }
        [ForeignKey("CommentId")]
        public Comment? Comment { get; set; }
    }
}
