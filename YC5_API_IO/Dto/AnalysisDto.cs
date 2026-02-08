using System;

namespace YC5_API_IO.Dto
{
    public class AnalysisDto
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime AnalysisDate { get; set; }
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int TotalComments { get; set; }
        public int TotalCategories { get; set; }
        public int TotalCountdowns { get; set; }
        public DateTime? LastActivityDate { get; set; }
    }
}
