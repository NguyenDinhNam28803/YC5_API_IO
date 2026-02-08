using System;
using System.ComponentModel.DataAnnotations;

namespace YC5_API_IO.Dto
{
    public class CreateCountdownDto
    {
        [Required(ErrorMessage = "Countdown Name is required.")]
        [StringLength(50, ErrorMessage = "Countdown Name cannot exceed 50 characters.")]
        public string CountDownName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Countdown Description is required.")]
        [StringLength(50, ErrorMessage = "Countdown Description cannot exceed 50 characters.")]
        public string CountDownDescription { get; set; } = string.Empty;

        [Required(ErrorMessage = "Target Date is required.")]
        public DateTime TargetDate { get; set; } = DateTime.UtcNow;
    }
}
