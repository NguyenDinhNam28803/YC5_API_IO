using System;
using System.ComponentModel.DataAnnotations;

namespace YC5_API_IO.Dto
{
    public class UpdateCountdownDto
    {
        [StringLength(50, ErrorMessage = "Countdown Name cannot exceed 50 characters.")]
        public string? CountDownName { get; set; }

        [StringLength(50, ErrorMessage = "Countdown Description cannot exceed 50 characters.")]
        public string? CountDownDescription { get; set; }

        public string? CountDownStatus { get; set; } // Represent enum as string for DTO

        public DateTime? TargetDate { get; set; }
    }
}
