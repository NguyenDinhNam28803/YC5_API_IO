using System;
using System.ComponentModel.DataAnnotations;
using YC5_API_IO.Models; // For TaskStatus and PriorityLevel enums

namespace YC5_API_IO.Dto
{
    public class CreateTaskDto
    {
        [Required(ErrorMessage = "Category ID is required.")]
        [StringLength(50, ErrorMessage = "Category ID cannot exceed 50 characters.")]
        public string CategoryId { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Parent Task ID cannot exceed 50 characters.")]
        public string? ParentTaskId { get; set; }

        [StringLength(250, ErrorMessage = "Task description cannot exceed 250 characters.")]
        public string TaskDescription { get; set; } = string.Empty;

        [Required(ErrorMessage = "Task Name is required.")]
        [StringLength(50, ErrorMessage = "Task Name cannot exceed 50 characters.")]
        public string TaskName { get; set; } = string.Empty;

        public Models.TaskStatus TaskStatus { get; set; } = Models.TaskStatus.InProgress;

        public PriorityLevel TaskPriority { get; set; } = PriorityLevel.Low;

        public DateTime DueDate { get; set; } = DateTime.UtcNow.AddDays(7); // Default to 7 days from now

        public List<string> TagNames { get; set; } = new List<string>();
    }
}
