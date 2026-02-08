using System;
using System.ComponentModel.DataAnnotations;

namespace YC5_API_IO.Dto
{
    public class TagDto
    {
        public string TagId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string TagName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
