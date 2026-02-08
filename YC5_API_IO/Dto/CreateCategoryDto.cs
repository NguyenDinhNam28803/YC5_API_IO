using System.ComponentModel.DataAnnotations;

namespace YC5_API_IO.Dto
{
    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(50, ErrorMessage = "Category name cannot exceed 50 characters.")]
        public string CategoryName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category description is required.")]
        [StringLength(50, ErrorMessage = "Category description cannot exceed 50 characters.")]
        public string CategoryDescription { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Color cannot exceed 50 characters.")]
        public string Color { get; set; } = "Gray";
    }
}
