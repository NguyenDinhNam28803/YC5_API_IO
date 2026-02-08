using System;
using System.ComponentModel.DataAnnotations;

namespace YC5_API_IO.Dto
{
    public class CreateNotificationDto
    {
        [Required(ErrorMessage = "Message is required.")]
        [StringLength(500, ErrorMessage = "Message cannot exceed 500 characters.")]
        public string Message { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Related Entity Type cannot exceed 50 characters.")]
        public string? RelatedEntityType { get; set; }

        [StringLength(50, ErrorMessage = "Related Entity ID cannot exceed 50 characters.")]
        public string? RelatedEntityId { get; set; }
    }
}
