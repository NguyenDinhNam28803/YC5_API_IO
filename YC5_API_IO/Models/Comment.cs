using System.ComponentModel.DataAnnotations;

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
        public string CommentTitle { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string CommentText { get; set; } = string.Empty;

        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        // Navigation property for related Attachments
        public ICollection<Attachment>? Attachments { get; set; }
    }
}
