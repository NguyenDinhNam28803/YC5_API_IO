using System.ComponentModel.DataAnnotations;

namespace YC5_API_IO.Models
{
    public enum TaskStatus
    {
        InProgress,
        Completed,
    }

    public enum PriorityLevel
    {
        Low,
        Medium,
        High,
    }

    public class Task
    {
        [Key]
        [Required]
        [StringLength(50)]
        public string TaskId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(50)]
        public string CategoryId { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string UserId { get; set; } = string.Empty;

        [StringLength(50)]
        public string? ParentTaskId { get; set; }

        [Required]
        [StringLength(50)]
        public string TaskName { get; set; } = string.Empty;

        [StringLength(250)]
        public string TaskDescription { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public TaskStatus TaskStatus { get; set; } = TaskStatus.InProgress;

        [StringLength(50)]
        public PriorityLevel Status { get; set; } = PriorityLevel.Low;

        [Required]
        [StringLength(50)]
        public DateTime DueDate { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(50)]
        public string CompletedAt { get; set; } = string.Empty;

        [StringLength(50)]
        public string UpdatedAt { get; set; } = string.Empty;

        // Navigation property for related Tags
        public ICollection<Tag>? Tags { get; set; }

        // Navigation property for related Comments
        public ICollection<Comment>? Comments { get; set; }

        // Navigation property for related SubTasks
        public ICollection<Task>? SubTasks { get; set; }

        // Navigation property for related Attachments
        public ICollection<Attachment>? Attachments { get; set; }
    }
}
