using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using YC5_API_IO.Interfaces;
using YC5_API_IO.Dto;
using YC5_API_IO.Models;
using System.Reflection;

namespace YC5_API_IO.Services
{
    public class ExcelService : IExcelService
    {
        private readonly ITaskInterface _taskService;
        private readonly IUserInterface _userService;
        private readonly ICategoryInterface _categoryService;

        public ExcelService(ITaskInterface taskService, IUserInterface userService, ICategoryInterface categoryService)
        {
            _taskService = taskService;
            _userService = userService;
            _categoryService = categoryService;
        }

        public async Task<ImportTasksResult> ImportTasksFromExcelAsync(IFormFile file)
        {
            var tasksToProcess = new List<TaskToImport>();
            var initialErrors = new List<ImportError>();

            if (file == null || file.Length == 0)
            {
                initialErrors.Add(new ImportError { Message = "File not selected or is empty." });
                return new ImportTasksResult { Errors = initialErrors };
            }

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets.FirstOrDefault();

            if (worksheet == null)
            {
                initialErrors.Add(new ImportError { Message = "Excel file contains no worksheets." });
                return new ImportTasksResult { Errors = initialErrors };
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

            // Updated required headers
            var requiredHeaders = new[] { "TaskName", "UserName", "CategoryName" };
            foreach (var requiredHeader in requiredHeaders)
            {
                if (!headerMap.ContainsKey(requiredHeader))
                {
                    initialErrors.Add(new ImportError { Row = 0, Message = $"Missing required header: '{requiredHeader}'" });
                }
            }

            if (initialErrors.Any())
            {
                return new ImportTasksResult { Errors = initialErrors };
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

                // Validate required fields in ExcelTaskImportDto (now for UserName and CategoryName)
                if (string.IsNullOrWhiteSpace(excelTaskDto.TaskName)) rowErrors.Add("TaskName is required.");
                if (string.IsNullOrWhiteSpace(excelTaskDto.UserName)) rowErrors.Add("UserName is required.");
                if (string.IsNullOrWhiteSpace(excelTaskDto.CategoryName)) rowErrors.Add("CategoryName is required.");

                // Convert ExcelTaskImportDto to CreateTaskDto
                var createTaskDto = new CreateTaskDto
                {
                    TaskName = excelTaskDto.TaskName,
                    // CategoryId and UserId will be looked up
                    // ParentTaskId will be resolved later in the multi-pass loop
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

                tasksToProcess.Add(new TaskToImport
                {
                    Dto = createTaskDto,
                    ExcelParentTaskIdentifier = excelTaskDto.ExcelParentTaskIdentifier,
                    RowNumber = rowNum,
                    Errors = rowErrors,
                    UserName = excelTaskDto.UserName, // Store UserName for lookup
                    CategoryName = excelTaskDto.CategoryName // Store CategoryName for lookup
                });
            }

            // Process tasks with a multi-pass approach to handle parent-child dependencies
            var taskNameIdMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase); // Maps Excel TaskName to generated TaskId
            var createdModelTasks = new List<YC5_API_IO.Models.Task>(); // Temporary list for models
            var allImportErrors = new List<ImportError>();

            int maxPasses = tasksToProcess.Count + 1; // Max passes to detect circular dependencies
            bool importedInPass;

            for (int pass = 0; pass < maxPasses; pass++)
            {
                importedInPass = false;
                foreach (var taskToImport in tasksToProcess.Where(t => !t.IsImported && !t.Errors.Any()).ToList()) // Only process unimported tasks without parsing errors
                {
                    // --- User and Category Lookup ---
                    Models.User? user = null;
                    if (!string.IsNullOrWhiteSpace(taskToImport.UserName))
                    {
                        user = await _userService.GetUserByNameAsync(taskToImport.UserName);
                        if (user == null)
                        {
                            taskToImport.Errors.Add($"User with name '{taskToImport.UserName}' not found.");
                            continue; // Skip this task for this pass
                        }
                        taskToImport.Dto.UserId = user.UserId; // Assign UserId to DTO
                    }
                    else
                    {
                        taskToImport.Errors.Add("UserName is required for task import.");
                        continue;
                    }

                    Models.Category? category = null;
                    if (!string.IsNullOrWhiteSpace(taskToImport.CategoryName))
                    {
                        category = await _categoryService.GetCategoryByNameAsync(user.UserId, taskToImport.CategoryName);
                        if (category == null)
                        {
                            taskToImport.Errors.Add($"Category with name '{taskToImport.CategoryName}' not found for user '{taskToImport.UserName}'.");
                            continue; // Skip this task for this pass
                        }
                        taskToImport.Dto.CategoryId = category.CategoryId; // Assign CategoryId to DTO
                    }
                    else
                    {
                        taskToImport.Errors.Add("CategoryName is required for task import.");
                        continue;
                    }

                    // If no parent identifier, or parent already imported
                    if (string.IsNullOrWhiteSpace(taskToImport.ExcelParentTaskIdentifier) ||
                        taskNameIdMap.ContainsKey(taskToImport.ExcelParentTaskIdentifier))
                    {
                        // Resolve ParentTaskId if it exists
                        if (!string.IsNullOrWhiteSpace(taskToImport.ExcelParentTaskIdentifier))
                        {
                            taskToImport.Dto.ParentTaskId = taskNameIdMap[taskToImport.ExcelParentTaskIdentifier];
                        }

                        var createdTask = await _taskService.CreateTaskAsync(user.UserId, taskToImport.Dto); // Use looked-up UserId
                        createdModelTasks.Add(createdTask); // Added to temporary list
                        taskNameIdMap[taskToImport.Dto.TaskName] = createdTask.TaskId; // Store mapping
                        taskToImport.IsImported = true;
                        importedInPass = true;
                    }
                }

                if (!importedInPass && tasksToProcess.Any(t => !t.IsImported && !t.Errors.Any()))
                {
                    // No tasks were imported in this pass, but there are still unimported tasks without errors.
                    // This indicates unresolvable dependencies (e.g., missing parent, circular reference).
                    break;
                }
                else if (!tasksToProcess.Any(t => !t.IsImported && !t.Errors.Any()))
                {
                    // All tasks without parsing errors have been imported.
                    break;
                }
            }

            // Collect all remaining errors (parsing errors + unresolvable dependencies)
            foreach (var taskToImport in tasksToProcess)
            {
                if (taskToImport.Errors.Any())
                    allImportErrors.Add(new ImportError { Row = taskToImport.RowNumber, Messages = taskToImport.Errors });
                else if (!taskToImport.IsImported)
                    allImportErrors.Add(new ImportError { Row = taskToImport.RowNumber, Messages = new List<string> { $"Task '{taskToImport.Dto.TaskName}' could not be imported due to unresolvable parent dependency or other unknown issue." } });
            }

            var successfullyImportedTaskDtos = createdModelTasks.Select(MapTaskToTaskDto).ToList();

            return new ImportTasksResult
            {
                ImportedCount = successfullyImportedTaskDtos.Count,
                Errors = allImportErrors,
                SuccessfullyImportedTasks = successfullyImportedTaskDtos
            };
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

        private PriorityLevel ParsePriorityLevel(string? priorityString, string errorMessage, List<string> errors)
        {
            if (string.IsNullOrWhiteSpace(priorityString) || !Enum.TryParse<PriorityLevel>(priorityString, true, out var priority))
            {
                errors.Add(errorMessage);
                return PriorityLevel.Low; // Default in case of error or empty string
            }
            return priority;
        }

        private class TaskToImport
        {
            public CreateTaskDto Dto { get; set; } = new CreateTaskDto();
            public string? ExcelParentTaskIdentifier { get; set; }
            public int RowNumber { get; set; }
            public bool IsImported { get; set; } = false;
            public List<string> Errors { get; set; } = new List<string>();
            public string? UserName { get; set; } // Added for lookup
            public string? CategoryName { get; set; } // Added for lookup
        }

        private TaskDto MapTaskToTaskDto(YC5_API_IO.Models.Task modelTask)
        {
            return new TaskDto
            {
                TaskId = modelTask.TaskId,
                CategoryId = modelTask.CategoryId,
                UserId = modelTask.UserId,
                ParentTaskId = modelTask.ParentTaskId,
                TaskName = modelTask.TaskName,
                TaskDescription = modelTask.TaskDescription,
                TaskStatus = modelTask.TaskStatus,
                TaskPriority = modelTask.TaskPriority,
                DueDate = modelTask.DueDate,
                CompletedAt = modelTask.CompletedAt,
                UpdatedAt = modelTask.UpdatedAt
            };
        }
    }
}
