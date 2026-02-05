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
        public TaskStatus TaskStatus { get; set; } = TaskStatus.InProgress;

        public PriorityLevel TaskPriority { get; set; } = PriorityLevel.Low;

        public DateTime DueDate { get; set; } = DateTime.UtcNow;

        public DateTime? CompletedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

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
