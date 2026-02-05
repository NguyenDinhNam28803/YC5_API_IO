using System.ComponentModel.DataAnnotations;

namespace YC5_API_IO.Models
{
    public class Role
    {
        [Key]
        [Required]
        [StringLength(50)]
        public string RoleId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(100)]
        public string RoleName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string RoleDescription { get; set; } = string.Empty;

        // Relationships can be added here if needed
        public ICollection<User>? Users { get; set; }
    }
}
