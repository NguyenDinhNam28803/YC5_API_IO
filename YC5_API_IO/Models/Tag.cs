using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YC5_API_IO.Models
{
    public class Tag
    {
        [Key]
        [Required]
        [StringLength(50)]
        public string TagId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(50)]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Required]
        [StringLength(50)]
        public string TagName { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;

        // Navigation property for many-to-many relationship with Task
        public ICollection<TaskTag>? TaskTags { get; set; }
    }
}