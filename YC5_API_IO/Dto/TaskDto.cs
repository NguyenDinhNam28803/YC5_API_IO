using System;
using YC5_API_IO.Models; // For TaskStatus and PriorityLevel enums

namespace YC5_API_IO.Dto
{
    public class TaskDto
    {
        public string TaskId { get; set; } = string.Empty;
        public string CategoryId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string? ParentTaskId { get; set; }
        public string TaskName { get; set; } = string.Empty;
        public string TaskDescription { get; set; } = string.Empty;
        public Models.TaskStatus TaskStatus { get; set; }
        public PriorityLevel TaskPriority { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
