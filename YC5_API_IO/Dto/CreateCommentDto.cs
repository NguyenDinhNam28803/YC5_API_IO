using System.ComponentModel.DataAnnotations;

namespace YC5_API_IO.Dto
{
    public class CreateCommentDto
    {
        [Required(ErrorMessage = "Task ID is required.")]
        public string TaskId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Comment Title is required.")]
        [StringLength(50, ErrorMessage = "Comment Title cannot exceed 50 characters.")]
        public string CommentTitle { get; set; } = string.Empty;

        [Required(ErrorMessage = "Comment Text is required.")]
        [StringLength(50, ErrorMessage = "Comment Text cannot exceed 50 characters.")]
        public string CommentText { get; set; } = string.Empty;
    }
}
