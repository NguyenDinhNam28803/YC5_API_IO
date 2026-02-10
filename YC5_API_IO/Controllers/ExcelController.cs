using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using YC5_API_IO.Interfaces;
using YC5_API_IO.Dto;
using YC5_API_IO.Models; // For TaskStatus and PriorityLevel enums
using System.Reflection;
using Microsoft.AspNetCore.Authorization;

namespace YC5_API_IO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelController : ControllerBase
    {
        private readonly ITaskInterface _taskService;

        public ExcelController(ITaskInterface taskService)
        {
            _taskService = taskService;
        }

        [Authorize]
        [HttpPost("import/tasks")]
        [ProducesResponseType(200, Type = typeof(ImportTasksResult))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ImportTasks(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File not selected or is empty.");
            }

            var importedTasks = new List<YC5_API_IO.Models.Task>();
            var errors = new List<ImportError>();

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets.FirstOrDefault();

            if (worksheet == null)
            {
                return BadRequest("Excel file contains no worksheets.");
            }

            // Get header row
            var headerCells = worksheet.Cells[worksheet.Dimension.Start.Row, worksheet.Dimension.Start.Column, 1, worksheet.Dimension.End.Column];
            var headerMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            foreach (var cell in headerCells)
            {
                if (!string.IsNullOrWhiteSpace(cell.Text))
                {
                    headerMap[cell.Text.Trim()] = cell.Start.Column;
                }
            }

            var requiredHeaders = new[] { "TaskName", "CategoryId", "UserId" };
            foreach (var requiredHeader in requiredHeaders)
            {
                if (!headerMap.ContainsKey(requiredHeader))
                {
                    errors.Add(new ImportError { Row = 0, Message = $"Missing required header: '{requiredHeader}'" });
                }
            }

            if (errors.Any())
            {
                return BadRequest(new { Message = "Missing required headers", Errors = errors });
            }

            // Get properties of ExcelTaskImportDto for mapping
            var dtoProperties = typeof(ExcelTaskImportDto).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            for (int rowNum = worksheet.Dimension.Start.Row + 1; rowNum <= worksheet.Dimension.End.Row; rowNum++)
            {
                var rowErrors = new List<string>();
                var excelTaskDto = new ExcelTaskImportDto();

                foreach (var prop in dtoProperties)
                {
                    if (headerMap.TryGetValue(prop.Name, out int colIndex))
                    {
                        var cellValue = worksheet.Cells[rowNum, colIndex].Text?.Trim();
                        try
                        {
                            prop.SetValue(excelTaskDto, cellValue);
                        }
                        catch (Exception ex)
                        {
                            rowErrors.Add($"Column '{prop.Name}': Error setting value '{cellValue}'. {ex.Message}");
                        }
                    }
                }

                // Validate required fields in ExcelTaskImportDto
                if (string.IsNullOrWhiteSpace(excelTaskDto.TaskName)) rowErrors.Add("TaskName is required.");
                if (string.IsNullOrWhiteSpace(excelTaskDto.CategoryId)) rowErrors.Add("CategoryId is required.");
                if (string.IsNullOrWhiteSpace(excelTaskDto.UserId)) rowErrors.Add("UserId is required.");

                if (rowErrors.Any())
                {
                    errors.Add(new ImportError { Row = rowNum, Messages = rowErrors });
                    continue;
                }

                // Convert ExcelTaskImportDto to CreateTaskDto
                var createTaskDto = new CreateTaskDto
                {
                    TaskName = excelTaskDto.TaskName,
                    CategoryId = excelTaskDto.CategoryId,
                    ParentTaskId = excelTaskDto.ParentTaskId,
                    TaskDescription = excelTaskDto.TaskDescription ?? string.Empty,
                    TagNames = excelTaskDto.TagNames?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList() ?? new List<string>()
                };

                // --- Parsing Enums ---
                createTaskDto.TaskStatus = ParseTaskStatus(excelTaskDto.TaskStatus, $"Invalid TaskStatus value '{excelTaskDto.TaskStatus}'.", rowErrors);
                createTaskDto.TaskPriority = ParsePriorityLevel(excelTaskDto.TaskPriority, $"Invalid TaskPriority value '{excelTaskDto.TaskPriority}'.", rowErrors);

                // --- Parsing DueDate ---
                if (!string.IsNullOrWhiteSpace(excelTaskDto.DueDate))
                {
                    if (DateTime.TryParse(excelTaskDto.DueDate, out var dueDate))
                    {
                        createTaskDto.DueDate = dueDate;
                    }
                    else
                    {
                        rowErrors.Add($"Invalid DueDate value '{excelTaskDto.DueDate}'. Expected a valid date/time format.");
                    }
                }

                if (rowErrors.Any())
                {
                    errors.Add(new ImportError { Row = rowNum, Messages = rowErrors });
                    continue;
                }

                try
                {
                    var createdTask = await _taskService.CreateTaskAsync(excelTaskDto.UserId, createTaskDto);
                    importedTasks.Add(createdTask);
                }
                catch (Exception ex)
                {
                    errors.Add(new ImportError { Row = rowNum, Messages = new List<string> { $"Error creating task: {ex.Message}" } });
                }
            }

            return Ok(new ImportTasksResult
            {
                ImportedCount = importedTasks.Count,
                Errors = errors,
                SuccessfullyImportedTasks = importedTasks
            });
        }
        
        private Models.TaskStatus ParseTaskStatus(string? statusString, string errorMessage, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(statusString) || !Enum.TryParse<Models.TaskStatus>(statusString, true, out var status))
            {
                errors.Add(errorMessage);
                return Models.TaskStatus.InProgress; // Default in case of error or empty string
            }
            return status;
        }

        private Models.PriorityLevel ParsePriorityLevel(string? priorityString, string errorMessage, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(priorityString) || !Enum.TryParse<Models.PriorityLevel>(priorityString, true, out var priority))
            {
                errors.Add(errorMessage);
                return Models.PriorityLevel.Low; // Default in case of error or empty string
            }
            return priority;
        }
    }

    public class ImportTasksResult
    {
        public int ImportedCount { get; set; }
        public List<ImportError> Errors { get; set; } = new List<ImportError>();
        public List<YC5_API_IO.Models.Task> SuccessfullyImportedTasks { get; set; } = new List<YC5_API_IO.Models.Task>();
    }

    public class ImportError
    {
        public int Row { get; set; } // 0 for general errors, >0 for specific row errors
        public List<string> Messages { get; set; } = new List<string>();
        public string Message { get; set; } = string.Empty; // For general errors not tied to a specific row
    }
}
