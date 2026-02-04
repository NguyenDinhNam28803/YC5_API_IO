using System.ComponentModel.DataAnnotations;

namespace YC5_API_IO.Models
{
    public class Category
    {
        [Key]
        [Required]
        public string? CategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string CategoryDescription { get; set; } = string.Empty;

        [StringLength(50)]
        public string Color { get; set; } = "Gray";

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
