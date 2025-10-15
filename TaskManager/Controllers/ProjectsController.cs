using Microsoft.AspNetCore.Mvc;
using TaskManager.DTOs;
using TaskManager.Services;

namespace TaskManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ITaskService _taskService;
        private const string USER_ID_HEADER = "X-User-Id";

        public ProjectsController(IProjectService projectService, ITaskService taskService)
        {
            _projectService = projectService;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectResponse>>> GetUserProjects()
        {
            var userId = GetUserId();
            var projects = await _projectService.GetUserProjectsAsync(userId);
            return Ok(projects);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateProject([FromBody] CreateProjectRequest request)
        {
            var userId = GetUserId();
            var newProject = await _projectService.CreateProjectAsync(request, userId);
            return Ok(newProject);
        }

        [HttpGet("{projectId}/tasks")]
        public async Task<ActionResult<IEnumerable<TaskResponse>>> GetProjectTasks(int projectId)
        {
            var userId = GetUserId();
            var tasks = await _taskService.GetProjectTasksAsync(projectId, userId);
            return Ok(tasks);
        }

        [HttpPost("{projectId}/tasks")]
        public async Task<ActionResult<int>> CreateTask(int projectId, [FromBody] CreateTaskRequest request)
        {
            var userId = GetUserId();
            var taskId = await _taskService.CreateTaskAsync(projectId, request, userId);
            return Ok(taskId);
        }
    }
}