using Microsoft.EntityFrameworkCore;
using YC5_API_IO.Data;
using YC5_API_IO.Dto;
using YC5_API_IO.Interfaces;
using YC5_API_IO.Models;
using Task = YC5_API_IO.Models.Task; // Alias to avoid ambiguity with System.Threading.Tasks.Task

namespace YC5_API_IO.Services
{
    public class TaskService : ITaskInterface
    {
        private readonly ApplicationDbContext _context;
        private readonly ICategoryInterface _categoryService;

        public TaskService(ApplicationDbContext context, ICategoryInterface categoryService)
        {
            _context = context;
            _categoryService = categoryService;
        }

        public async Task<IEnumerable<Task>> GetTasksAsync(string userId)
        {
            return await _context.Tasks
                                 .Where(t => t.UserId == userId)
                                 .Include(t => t.Category) // Eager load category
                                 .Include(t => t.TaskTags)!
                                 .ThenInclude(tt => tt.Tag) // Eager load tags
                                 .ToListAsync();
        }

        public async Task<Task?> GetTaskByIdAsync(string userId, string taskId)
        {
            return await _context.Tasks
                                 .Where(t => t.UserId == userId && t.TaskId == taskId)
                                 .Include(t => t.Category)
                                 .Include(t => t.TaskTags)!
                                 .ThenInclude(tt => tt.Tag)
                                 .FirstOrDefaultAsync();
        }

        public async Task<Task> CreateTaskAsync(string userId, CreateTaskDto createTaskDto)
        {
            // Validate CategoryId
            if (!await _categoryService.CategoryExistsAsync(userId, createTaskDto.CategoryId))
            {
                throw new InvalidOperationException("Category not found or does not belong to the user.");
            }

            var task = new Task
            {
                TaskId = Guid.NewGuid().ToString(),
                UserId = userId,
                CategoryId = createTaskDto.CategoryId,
                ParentTaskId = createTaskDto.ParentTaskId,
                TaskName = createTaskDto.TaskName,
                TaskDescription = createTaskDto.TaskDescription,
                TaskStatus = createTaskDto.TaskStatus,
                TaskPriority = createTaskDto.TaskPriority,
                DueDate = createTaskDto.DueDate,
                UpdatedAt = DateTime.UtcNow
            };

            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<Task?> UpdateTaskAsync(string userId, string taskId, UpdateTaskDto updateTaskDto)
        {
            var task = await GetTaskByIdAsync(userId, taskId);

            if (task == null)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(updateTaskDto.CategoryId))
            {
                // Validate new CategoryId if provided
                if (!await _categoryService.CategoryExistsAsync(userId, updateTaskDto.CategoryId))
                {
                    throw new InvalidOperationException("New Category not found or does not belong to the user.");
                }
                task.CategoryId = updateTaskDto.CategoryId;
            }

            if (!string.IsNullOrEmpty(updateTaskDto.ParentTaskId))
            {
                task.ParentTaskId = updateTaskDto.ParentTaskId;
            }

            if (!string.IsNullOrEmpty(updateTaskDto.TaskName))
            {
                task.TaskName = updateTaskDto.TaskName;
            }

            if (!string.IsNullOrEmpty(updateTaskDto.TaskDescription))
            {
                task.TaskDescription = updateTaskDto.TaskDescription;
            }

            if (updateTaskDto.TaskStatus.HasValue)
            {
                task.TaskStatus = updateTaskDto.TaskStatus.Value;
                if (task.TaskStatus == Models.TaskStatus.Completed && !task.CompletedAt.HasValue)
                {
                    task.CompletedAt = DateTime.UtcNow;
                }
            }

            if (updateTaskDto.TaskPriority.HasValue)
            {
                task.TaskPriority = updateTaskDto.TaskPriority.Value;
            }

            if (updateTaskDto.DueDate.HasValue)
            {
                task.DueDate = updateTaskDto.DueDate.Value;
            }

            task.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> DeleteTaskAsync(string userId, string taskId)
        {
            var task = await GetTaskByIdAsync(userId, taskId);
            if (task == null)
            {
                return false;
            }

            _context.Tasks.Remove(task); // Hard delete for tasks
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> TaskExistsAsync(string userId, string taskId)
        {
            return await _context.Tasks.AnyAsync(t => t.UserId == userId && t.TaskId == taskId);
        }

        public async Task<IEnumerable<Task>> GetTasksByCategoryIdAsync(string userId, string categoryId)
        {
            return await _context.Tasks
                                 .Where(t => t.UserId == userId && t.CategoryId == categoryId)
                                 .Include(t => t.Category)
                                 .Include(t => t.TaskTags)!
                                 .ThenInclude(tt => tt.Tag)
                                 .ToListAsync();
        }
    }
}
