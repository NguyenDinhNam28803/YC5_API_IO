using YC5_API_IO.Models;
using YC5_API_IO.Dto;

namespace YC5_API_IO.Interfaces
{
    public interface ITaskInterface
    {
        Task<IEnumerable<Models.Task>> GetTasksAsync(string userId);
        Task<Models.Task?> GetTaskByIdAsync(string userId, string taskId);
        Task<Models.Task> CreateTaskAsync(string userId, CreateTaskDto createTaskDto);
        Task<Models.Task?> UpdateTaskAsync(string userId, string taskId, UpdateTaskDto updateTaskDto);
        Task<bool> DeleteTaskAsync(string userId, string taskId); // Soft delete
        Task<bool> TaskExistsAsync(string userId, string taskId);
        Task<IEnumerable<Models.Task>> GetTasksByCategoryIdAsync(string userId, string categoryId);
    }
}
