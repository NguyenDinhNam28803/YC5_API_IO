using System.ComponentModel.DataAnnotations;

namespace YC5_API_IO.Models
{
    public enum CountDownStatus
    {
        Active,
        Inactive,
        Completed,
    }

    public class CountDown
    {
        [Key]
        [Required]
        [StringLength(50)]
                public string CountDownId { get; set;} = Guid.NewGuid().ToString();

        [Required]
        [StringLength(50)]
        public string UserId { get; set;} = string.Empty;

        [Required]
        [StringLength(50)]
        public string CountDownName { get; set;} = string.Empty;

        [Required]
        [StringLength(50)]
        public string CountDownDescription { get; set;} = string.Empty;

        public CountDownStatus CountDownStatus { get; set;} = CountDownStatus.Active;

        public DateTime TargetDate { get; set;} = DateTime.UtcNow;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
