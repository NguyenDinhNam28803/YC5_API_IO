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
        public string CountDownId { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string UserId { get; set;} = string.Empty;

        [Required]
        [StringLength(50)]
        public string CountDownName { get; set;} = string.Empty;

        [Required]
        [StringLength(50)]
        public string CountDownDescription { get; set;} = string.Empty;

        [Required]
        [StringLength(50)]
        public CountDownStatus CountDownStatus { get; set;} = CountDownStatus.Active;

        [Required]
        [StringLength(50)]
        public DateTime TargetDate { get; set;} = DateTime.UtcNow;
    }
}
