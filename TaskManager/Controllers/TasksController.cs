using Microsoft.AspNetCore.Mvc;
using TaskManager.DTOs;
using TaskManager.Services;

namespace TaskManager.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private const string USER_ID_HEADER = "X-User-Id";

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        private int GetUserId()
        {
            if (!Request.Headers.TryGetValue(USER_ID_HEADER, out var userIdHeader))
            {
                throw new UnauthorizedAccessException("O ID do usuário é obrigatório.");
            }

            if (!int.TryParse(userIdHeader, out int userId))
            {
                throw new UnauthorizedAccessException("ID de usuário inválido");
            }

            return userId;
        }

        [HttpPut("{taskId}")]
        public async Task<ActionResult> UpdateTask(int taskId, [FromBody] UpdateTaskRequest request)
        {
            var userId = GetUserId();
            await _taskService.UpdateTaskAsync(taskId, request, userId);
            return NoContent();
        }

        [HttpDelete("{taskId}")]
        public async Task<ActionResult> DeleteTask(int taskId)
        {
            var userId = GetUserId();
            await _taskService.DeleteTaskAsync(taskId, userId);
            return NoContent();
        }

        [HttpGet("{taskId}/history")]
        public async Task<ActionResult<IEnumerable<TaskHistoryResponse>>> GetTaskHistory(int taskId)
        {
            var userId = GetUserId();
            var history = await _taskService.GetTaskHistoryAsync(taskId, userId);
            return Ok(history);
        }

        [HttpPost("{taskId}/comments")]
        public async Task<ActionResult> AddComment(int taskId, [FromBody] string comment)
        {
            var userId = GetUserId();
            await _taskService.AddCommentToTaskAsync(taskId, comment, userId);
            return NoContent();
        }
    }
}