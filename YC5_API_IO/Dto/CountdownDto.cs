using System;

namespace YC5_API_IO.Dto
{
    public class CountdownDto
    {
        public string CountDownId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string CountDownName { get; set; } = string.Empty;
        public string CountDownDescription { get; set; } = string.Empty;
        public string CountDownStatus { get; set; } = string.Empty; // Represent enum as string for DTO
        public DateTime TargetDate { get; set; }
    }
}
