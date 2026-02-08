using System.ComponentModel.DataAnnotations;

namespace YC5_API_IO.Dto
{
    public class CreateTagDto
    {
        [Required(ErrorMessage = "Tag name is required.")]
        [StringLength(50, ErrorMessage = "Tag name cannot exceed 50 characters.")]
        public string TagName { get; set; } = string.Empty;
    }
}
