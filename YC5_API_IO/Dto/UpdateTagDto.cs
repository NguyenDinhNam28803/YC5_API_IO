using System.ComponentModel.DataAnnotations;

namespace YC5_API_IO.Dto
{
    public class UpdateTagDto
    {
        [StringLength(50, ErrorMessage = "Tag name cannot exceed 50 characters.")]
        public string? TagName { get; set; }
    }
}
