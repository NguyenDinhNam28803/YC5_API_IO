using System.Collections.Generic;

namespace YC5_API_IO.Dto
{
    public class ImportTasksResult
    {
        public int ImportedCount { get; set; }
        public List<ImportError> Errors { get; set; } = new List<ImportError>();
        public List<YC5_API_IO.Dto.TaskDto> SuccessfullyImportedTasks { get; set; } = new List<YC5_API_IO.Dto.TaskDto>();
    }

    public class ImportError
    {
        public int Row { get; set; } // 0 for general errors, >0 for specific row errors
        public List<string> Messages { get; set; } = new List<string>();
        public string Message { get; set; } = string.Empty; // For general errors not tied to a specific row
    }
}
