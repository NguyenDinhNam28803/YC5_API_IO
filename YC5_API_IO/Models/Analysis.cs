using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YC5_API_IO.Models
{
    public class Analysis
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Required]
        public DateTime AnalysisDate { get; set; } = DateTime.UtcNow;

        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int TotalComments { get; set; }
        public int TotalCategories { get; set; }
        public int TotalCountdowns { get; set; }

        public DateTime? LastActivityDate { get; set; }
    }
}
