using System;

namespace YC5_API_IO.Dto
{
    public class CommentDto
    {
        public string CommentId { get; set; } = string.Empty;
        public string TaskId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string CommentTitle { get; set; } = string.Empty;
        public string CommentText { get; set; } = string.Empty;
        public DateTime CreateAt { get; set; }
    }
}
