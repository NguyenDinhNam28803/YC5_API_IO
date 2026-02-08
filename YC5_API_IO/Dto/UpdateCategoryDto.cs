using System.ComponentModel.DataAnnotations;

namespace YC5_API_IO.Dto
{
    public class UpdateCategoryDto
    {
        [StringLength(50, ErrorMessage = "Category name cannot exceed 50 characters.")]
        public string? CategoryName { get; set; }

        [StringLength(50, ErrorMessage = "Category description cannot exceed 50 characters.")]
        public string? CategoryDescription { get; set; }

        [StringLength(50, ErrorMessage = "Color cannot exceed 50 characters.")]
        public string? Color { get; set; }
    }
}
