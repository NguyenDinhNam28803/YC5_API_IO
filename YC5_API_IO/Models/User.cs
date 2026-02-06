using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YC5_API_IO.Models
{
    public class User
    {
        [Key]
        [Required]
        [StringLength(50)]
        public string UserId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(50)]
        public string RoleId { get; set; } = string.Empty; // Foreign key for Role

        [ForeignKey("RoleId")]
        public Role? Role { get; set; } // Navigation property

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

        // Navigation property for related Notifications
        public ICollection<Notification>? Notifications { get; set; }

        // Navigation property for related Comments
        public ICollection<Comment>? Comments { get; set; }

        // Navigation property for attachments uploaded by this user
        public ICollection<Attachment>? UploadedAttachments { get; set; }
    }
}
