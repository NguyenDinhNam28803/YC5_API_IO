using System.ComponentModel.DataAnnotations;

namespace YC5_API_IO.Models
{
    public class Tag
    {
        [Key]
        [Required]
        [StringLength(50)]
        public string TagId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(50)]
        public string TagName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string TaskId { get; set; } = string.Empty;
    }
}
