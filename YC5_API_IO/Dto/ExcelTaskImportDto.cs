using System;
using System.ComponentModel.DataAnnotations;

namespace YC5_API_IO.Dto
{
    public class ExcelTaskImportDto
    {
        [Required(ErrorMessage = "Task Name is required.")]
        [StringLength(50, ErrorMessage = "Task Name cannot exceed 50 characters.")]
        public string TaskName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category ID is required.")]
        [StringLength(50, ErrorMessage = "Category ID cannot exceed 50 characters.")]
        public string CategoryId { get; set; } = string.Empty;

        [Required(ErrorMessage = "User ID is required.")]
        [StringLength(50, ErrorMessage = "User ID cannot exceed 50 characters.")]
        public string UserId { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "Task description cannot exceed 250 characters.")]
        public string? TaskDescription { get; set; }

        public string? TaskStatus { get; set; } // Will be parsed to Models.TaskStatus

        public string? TaskPriority { get; set; } // Will be parsed to Models.PriorityLevel

        public string? DueDate { get; set; } // Will be parsed to DateTime

        [StringLength(50, ErrorMessage = "Parent Task ID cannot exceed 50 characters.")]
        public string? ParentTaskId { get; set; }

        public string? TagNames { get; set; } // Comma-separated string
    }
}
