using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YC5_API_IO.Dto;
using YC5_API_IO.Interfaces;
using YC5_API_IO.Models;
using Task = YC5_API_IO.Models.Task; // Alias to avoid ambiguity with System.Threading.Tasks.Task

namespace YC5_API_IO.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Requires authentication
    public class TasksController : ControllerBase
    {
        private readonly ITaskInterface _taskService;

        public TasksController(ITaskInterface taskService)
        {
            _taskService = taskService;
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User ID not found.");
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasks()
        {
            var userId = GetUserId();
            var tasks = await _taskService.GetTasksAsync(userId);
            return Ok(tasks.Select(t => new TaskDto
            {
                TaskId = t.TaskId,
                CategoryId = t.CategoryId,
                UserId = t.UserId,
                ParentTaskId = t.ParentTaskId,
                TaskName = t.TaskName,
                TaskDescription = t.TaskDescription,
                TaskStatus = t.TaskStatus,
                TaskPriority = t.TaskPriority,
                DueDate = t.DueDate,
                CompletedAt = t.CompletedAt,
                UpdatedAt = t.UpdatedAt
            }));
        }

        // GET: api/Tasks/{taskId}
        [HttpGet("{taskId}")]
        public async Task<ActionResult<TaskDto>> GetTask(string taskId)
        {
            var userId = GetUserId();
            var task = await _taskService.GetTaskByIdAsync(userId, taskId);

            if (task == null)
            {
                return NotFound();
            }

            return Ok(new TaskDto
            {
                TaskId = task.TaskId,
                CategoryId = task.CategoryId,
                UserId = task.UserId,
                ParentTaskId = task.ParentTaskId,
                TaskName = task.TaskName,
                TaskDescription = task.TaskDescription,
                TaskStatus = task.TaskStatus,
                TaskPriority = task.TaskPriority,
                DueDate = task.DueDate,
                CompletedAt = task.CompletedAt,
                UpdatedAt = task.UpdatedAt
            });
        }

        // POST: api/Tasks
        [HttpPost]
        public async Task<ActionResult<TaskDto>> CreateTask(CreateTaskDto createTaskDto)
        {
            var userId = GetUserId();
            try
            {
                var task = await _taskService.CreateTaskAsync(userId, createTaskDto);
                return CreatedAtAction(nameof(GetTask), new { taskId = task.TaskId }, new TaskDto
                {
                    TaskId = task.TaskId,
                    CategoryId = task.CategoryId,
                    UserId = task.UserId,
                    ParentTaskId = task.ParentTaskId,
                    TaskName = task.TaskName,
                    TaskDescription = task.TaskDescription,
                    TaskStatus = task.TaskStatus,
                    TaskPriority = task.TaskPriority,
                    DueDate = task.DueDate,
                    CompletedAt = task.CompletedAt,
                    UpdatedAt = task.UpdatedAt
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Tasks/{taskId}
        [HttpPut("{taskId}")]
        public async Task<IActionResult> UpdateTask(string taskId, UpdateTaskDto updateTaskDto)
        {
            var userId = GetUserId();
            try
            {
                var updatedTask = await _taskService.UpdateTaskAsync(userId, taskId, updateTaskDto);

                if (updatedTask == null)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Tasks/{taskId}
        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask(string taskId)
        {
            var userId = GetUserId();
            var result = await _taskService.DeleteTaskAsync(userId, taskId);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // GET: api/Tasks/ByCategory/{categoryId}
        [HttpGet("ByCategory/{categoryId}")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasksByCategoryId(string categoryId)
        {
            var userId = GetUserId();
            var tasks = await _taskService.GetTasksByCategoryIdAsync(userId, categoryId);

            return Ok(tasks.Select(t => new TaskDto
            {
                TaskId = t.TaskId,
                CategoryId = t.CategoryId,
                UserId = t.UserId,
                ParentTaskId = t.ParentTaskId,
                TaskName = t.TaskName,
                TaskDescription = t.TaskDescription,
                TaskStatus = t.TaskStatus,
                TaskPriority = t.TaskPriority,
                DueDate = t.DueDate,
                CompletedAt = t.CompletedAt,
                UpdatedAt = t.UpdatedAt
            }));
        }
    }
}
