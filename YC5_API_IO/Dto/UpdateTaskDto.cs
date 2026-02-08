using System;
using System.ComponentModel.DataAnnotations;
using YC5_API_IO.Models; // For TaskStatus and PriorityLevel enums

namespace YC5_API_IO.Dto
{
    public class UpdateTaskDto
    {
        [StringLength(50, ErrorMessage = "Category ID cannot exceed 50 characters.")]
        public string? CategoryId { get; set; }

        [StringLength(50, ErrorMessage = "Parent Task ID cannot exceed 50 characters.")]
        public string? ParentTaskId { get; set; }

        [StringLength(50, ErrorMessage = "Task name cannot exceed 50 characters.")]
        public string? TaskName { get; set; }

        [StringLength(250, ErrorMessage = "Task description cannot exceed 250 characters.")]
        public string? TaskDescription { get; set; }

        public Models.TaskStatus? TaskStatus { get; set; }

        public PriorityLevel? TaskPriority { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? CompletedAt { get; set; }
    }
}
