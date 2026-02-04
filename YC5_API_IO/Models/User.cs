using System.ComponentModel.DataAnnotations;

namespace YC5_API_IO.Models
{
    public class User
    {
        [Key]
        [Required]
        public string? UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string PasswordHasshed { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdatedAt { get; set;} = DateTime.UtcNow;

        // Navigation property for related Categories
        public ICollection<Category>? Categories { get; set; }

        // Navigation property for related Tasks
        public ICollection<Task>? Tasks { get; set; }

        // Navigation property for related CountDowns
        public ICollection<CountDown>? CountDowns { get; set; }
    }
}
