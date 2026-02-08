using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YC5_API_IO.Models
{
    public class TaskTag
    {
        [Required]
        [StringLength(50)]
        public string TaskId { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string TagId { get; set; } = string.Empty;

        // Navigation properties
        [ForeignKey("TaskId")]
        public Task? Task { get; set; }

        [ForeignKey("TagId")]
        public Tag? Tag { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
