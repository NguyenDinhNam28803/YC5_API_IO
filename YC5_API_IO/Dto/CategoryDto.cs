using System.ComponentModel.DataAnnotations;

namespace YC5_API_IO.Dto
{
    public class CategoryDto
    {
        public string CategoryId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryDescription { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
